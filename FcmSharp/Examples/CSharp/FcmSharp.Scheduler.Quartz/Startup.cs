// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Scheduler.Quartz.Quartz.Jobs;
using FcmSharp.Scheduler.Quartz.Services;
using FcmSharp.Scheduler.Quartz.Testing;
using FcmSharp.Scheduler.Quartz.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FcmSharp.Scheduler.Quartz
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; set; }

        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Environment = env;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add a CORS Policy to allow "Everything":
            services.AddCors(o =>
            {
                o.AddPolicy("Everything", p =>
                {
                    p.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            services
                .AddOptions()
                .AddQuartz()
                .AddTransient<ProcessMessageJob>()
                .AddTransient<IFcmClient, MockFcmClient>()
                .AddTransient<ISchedulerService, SchedulerService>()
                .AddTransient<IMessagingService, MessagingService>()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.EnsureDatabaseCreated()
               .UseCors("Everything")
               .UseStaticFiles()
               .UseQuartz()
               .UseMvc();
        }
    }
}