// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Exceptions;
using FcmSharp.Http.Constants;
using FcmSharp.Http.Retry;
using FcmSharp.Model.Options;
using FcmSharp.Model.Topics;
using FcmSharp.Requests.Topics;
using FcmSharp.Responses;
using FcmSharp.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace FcmSharp.Test.Integration
{
    /// <summary>
    /// Creates a new FCM Endpoint URL for each Unit Test.
    /// </summary>
    public class IntegrationTestOptions : IFcmClientSettings
    {
        private readonly string testName;

        public IntegrationTestOptions(string testName)
        {
            this.testName = testName;
        }

        public string FcmUrl
        {
            get
            {
                return string.Format("http://localhost:8081/{0}", testName);
            }
        }

        public string ApiKey
        {
            get
            {
                return "Abc";
            }
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }

    /// <summary>
    /// A Controller, which "simulates" the FCM Server. It isn't a beauty, but works.
    /// </summary>
    public class FcmSampleController : Controller
    {
        public FcmSampleController()
        {
        }

        [HttpPost]
        [Route("TopicMessage_OK")]
        public JsonResult TopicMessage_OK(object message)
        {
            var response = new TopicMessageResponse
            {
                MessageId = 1234
            };
            return Json(response);
        }

        [HttpPost]
        [Route("TopicMessage_InternalServerError")]
        public IActionResult TopicMessage_InternalServerError(object message)
        {
            return StatusCode(500);
        }

        [HttpPost]
        [Route("TopicMessage_Unauthorized")]
        public IActionResult TopicMessage_Unauthorized(object message)
        {
            return StatusCode((int) HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        [Route("TopicMessage_WithRetry")]
        public JsonResult TopicMessage_WithRetry(object message)
        {
            // The Topic Message Response:
            var response = new TopicMessageResponse
            {
                MessageId = 1234
            };

            // Create the JSON Result:
            var result = new JsonResult(response);

            // Add Retry Header:
            this.HttpContext.Response.Headers[HttpHeaderNames.RetryAfter] = "Fri, 31 Dec 1999 23:59:59 GMT";

            // Set as Internal Server Error:
            this.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            return Json(response);
        }

        [HttpPost]
        [Route("TopicMessage_WithRetryTimeSpan")]
        public JsonResult TopicMessage_WithRetryTimeSpan(object message)
        {
            // The Topic Message Response:
            var response = new TopicMessageResponse
            {
                MessageId = 1234
            };

            // Create the JSON Result:
            var result = new JsonResult(response);

            // Add Retry Header:
            this.HttpContext.Response.Headers[HttpHeaderNames.RetryAfter] = "60";

            // Set as BadRequest:
            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return Json(response);
        }


        [HttpPost]
        [Route("TopicMessage_InternalServerError_WithoutRetry")]
        public JsonResult TopicMessage_InternalServerError_WithoutRetry(object message)
        {
            // The Topic Message Response:
            var response = new TopicMessageResponse
            {
                MessageId = 1234
            };

            // Create the JSON Result:
            var result = new JsonResult(response);

            // Set as InternalServerError:
            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return Json(response);
        }

        [HttpPost]
        [Route("TopicMessage_HasAuthHeader")]
        public IActionResult TopicMessage_HasAuthHeader(object message)
        {
            string value = this.HttpContext.Request.Headers[HttpHeaderNames.Authorization];

            if(string.IsNullOrWhiteSpace(value))
            {
                return BadRequest();
            }

            if(!value.Equals("key=Abc", StringComparison.Ordinal))
            {
                return BadRequest();
            }

            // The Topic Message Response:
            var response = new TopicMessageResponse
            {
                MessageId = 1234
            };

            // Create the JSON Result:
            return new JsonResult(response);
        }

    }

    [TestFixture]
    public class IntegrationTest
    {
        private IWebHost host;

        [SetUp]
        public void SetUp()
        {
            // Use Kestrel to Host the Controller:
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:8081")
                .UseContentRoot(Directory.GetCurrentDirectory());

            // Build the Host:
            this.host = builder.Build();

            // And... Ignite!
            host.Start();
        }

        [TearDown]
        public void TearDown()
        {
            host.Dispose();
        }

        [Test]
        public void TestServer()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_OK")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();

                Assert.AreEqual(1234, result.MessageId);
                Assert.AreEqual(null, result.ErrorCode);
            }
        }

        [Test]
        public void TestInternalServerError()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_InternalServerError")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                bool isInternalServerError = false;

                try
                {
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                }
                catch (FcmGeneralException)
                {
                    isInternalServerError = true;
                }

                Assert.AreEqual(true, isInternalServerError);
            }
        }


        [Test]
        public void TestTopicMessageWithRetryDateTimeOffset()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_WithRetry")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                bool isRetryExceptionCaught = false;

                try
                {
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                }
                catch (FcmRetryAfterException e)
                {
                    RetryConditionValue retryConditionValue = e.RetryConditionValue;

                    Assert.IsNotNull(retryConditionValue);
                    Assert.IsNull(retryConditionValue.Delta);

                    // Fri, 31 Dec 1999 23:59:59 GMT
                    Assert.AreEqual(new DateTimeOffset(1999, 12, 31, 23, 59, 59, TimeSpan.FromHours(0)), retryConditionValue.Date);

                    isRetryExceptionCaught = true;
                }

                Assert.AreEqual(true, isRetryExceptionCaught);
            }
        }

        [Test]
        public void TestTopicMessageWithRetryTimeSpan()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_WithRetryTimeSpan")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                bool isRetryExceptionCaught = false;

                try
                {
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                }
                catch (FcmRetryAfterException e)
                {
                    RetryConditionValue retryConditionValue = e.RetryConditionValue;

                    Assert.IsNotNull(retryConditionValue);
                    Assert.IsNull(retryConditionValue.Date);

                    // 1 Minute Delta
                    Assert.AreEqual(retryConditionValue.Delta, TimeSpan.FromMinutes(1));

                    isRetryExceptionCaught = true;
                }

                Assert.AreEqual(true, isRetryExceptionCaught);
            }
        }

        [Test]
        public void TestTopicMessageInternalServerErrorWithouRetry()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_InternalServerError_WithoutRetry")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                bool isRetryExceptionCaught = false;
                bool isGeneralExceptionCaught = false;

                try
                {
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                }
                catch (FcmRetryAfterException)
                {
                    isRetryExceptionCaught = true;
                }
                catch(FcmGeneralException)
                {
                    isGeneralExceptionCaught = true;
                }

                Assert.AreEqual(false, isRetryExceptionCaught);
                Assert.AreEqual(true, isGeneralExceptionCaught);
            }
        }

        [Test]
        public void TestUnauthorized()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_Unauthorized")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                bool isAuthenticationException = false;

                try
                {
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                }
                catch (FcmAuthenticationException)
                {
                    isAuthenticationException = true;
                }

                Assert.AreEqual(true, isAuthenticationException);
            }
        }

        [Test]
        public void TestAuthHeaderIsInRequest()
        {
            using (var client = new FcmClient(new IntegrationTestOptions("TopicMessage_HasAuthHeader")))
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var message = new TopicUnicastMessage<int>(new FcmMessageOptionsBuilder().Build(), new Topic("a"), 1);

                bool didThrow = false;

                try
                {
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();

                    Assert.IsNotNull(result);
                }
                catch (Exception)
                {
                    didThrow = true;
                }

                Assert.AreEqual(false, didThrow);
            }
        }

        

    }
}
