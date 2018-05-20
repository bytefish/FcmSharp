// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace FcmSharp.Test.Integration
{

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
    }
}
