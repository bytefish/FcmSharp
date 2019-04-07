// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.BackOff;
using FcmSharp.Exceptions;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Client;
using FcmSharp.Settings;
using Google.Apis.Http;
using Microsoft.AspNetCore.Mvc;

namespace FcmSharp.Test.Integration
{
    public static class GlobalState
    {
        public static int RequestNumber = 0;
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
    public class RetryableSampleController : Controller
    {
        public RetryableSampleController()
        {
        }

        [HttpGet]
        [Route("return503_UntilRequestFour")]
        public IActionResult Returns503Until()
        {
            // Request received:
            GlobalState.RequestNumber = GlobalState.RequestNumber + 1;

            // If this is Request 4, return HTTP Status 200:
            if (GlobalState.RequestNumber % 4 == 0)
            {
                return Ok();
            }

            return StatusCode(503);
        }
    }

    [TestFixture]
    public class IntegrationTest
    {
        private IWebHost host;

        [SetUp]
        public void SetUp()
        {
            // Reset Global Request Counter:
            GlobalState.RequestNumber = 0;

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
        
        [Test]
        [Description("This Test uses the Default Settings and should run for approximately 8 Seconds!")]
        public async Task ExponentialBackoff503Test()
        {
            // This needs to be a valid Service Account Credentials File. Can't mock it away:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"D:\serviceAccountKey.json");

            // Initialize a new FcmHttpClient to send to localhost:
            var client = new FcmHttpClient(settings);

            // Construct a Fake Message:
            var builder = new HttpRequestMessageBuilder("http://localhost:8081/return503_UntilRequestFour", HttpMethod.Get);

            CancellationToken longLivingCancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;

            await client.SendAsync(builder, longLivingCancellationToken);
        }

        [Test]
        [Description("This Test configures only 2 Retries for a 503 HTTP Status Code. " +
                     "It should fail, because the mock endpoint only reports success " +
                     "after 4 requests to the API.")]
        public async Task ExponentialBackoff503TooFewRetriesTest()
        {
            // Construct new ExponentialBackOffSettings:
            var exponentialBackOffSettings = new ExponentialBackOffSettings(2, TimeSpan.FromMilliseconds(250), TimeSpan.FromSeconds(30));

            // This needs to be a valid Service Account Credentials File. Can't mock it away:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"D:\serviceAccountKey.json", exponentialBackOffSettings);

            // Initialize a new FcmHttpClient to send to localhost:
            var client = new FcmHttpClient(settings);

            // Construct a Fake Message:
            var builder = new HttpRequestMessageBuilder("http://localhost:8081/return503_UntilRequestFour", HttpMethod.Get);

            CancellationToken longLivingCancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;

            bool fcmHttpExceptionWasThrown = false;

            try
            {
                await client.SendAsync(builder, longLivingCancellationToken);
            }
            catch (FcmHttpException)
            {
                fcmHttpExceptionWasThrown = true;
            }

            Assert.IsTrue(fcmHttpExceptionWasThrown);
        }


        [TearDown]
        public void TearDown()
        {
            host.Dispose();
        }
    }
}
