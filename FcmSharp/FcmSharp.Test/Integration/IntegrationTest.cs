// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Client;
using FcmSharp.Settings;
using Google.Apis.Http;

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
        [Route("return503")]
        public IActionResult Returns503()
        {
            // Request received:
            GlobalState.RequestNumber = GlobalState.RequestNumber + 1;

            // If this is the 3rd Request, exit:
            if (GlobalState.RequestNumber % 7 == 0)
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
        public async Task ExponentialBackoff503Test()
        {
            // This needs to be a valid Service Account Credentials File. Can't mock it away:
            var settings = FileBasedFcmClientSettings.CreateFromFile("project", @"D:\serviceAccountKey.json");

            // Initialize a new FcmHttpClient to send to localhost:
            var client = new FcmHttpClient(settings);

            // Construct a Fake Message:
            var builder = new HttpRequestMessageBuilder("http://localhost:8081/return503", HttpMethod.Get);

            CancellationToken longLivingCancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(30)).Token;

            await client.SendAsync(builder, longLivingCancellationToken);
        }

        [TearDown]
        public void TearDown()
        {
            host.Dispose();
        }
    }
}
