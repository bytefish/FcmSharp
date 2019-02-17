// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Scheduler.Quartz.Quartz.JobFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace FcmSharp.Scheduler.Quartz.Web.Extensions
{
    public static class QuartzExtensions
    {
        public static IApplicationBuilder UseQuartz(this IApplicationBuilder app)
        {
            var scheduler = app.ApplicationServices.GetService<IScheduler>();

            scheduler.Start().GetAwaiter().GetResult();

            return app;
        }

        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<IScheduler>(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

                scheduler.JobFactory = provider.GetService<IJobFactory>();

                return scheduler;
            });

            return services;
        }
    }
}
