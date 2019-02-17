// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Quartz;
using Quartz.Spi;

namespace FcmSharp.Scheduler.Quartz.Quartz.JobFactory
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider container;

        public JobFactory(IServiceProvider container)
        {
            this.container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobType = bundle.JobDetail.JobType;

            return container.GetService(jobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
