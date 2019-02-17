// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Quartz.Database;
using FcmSharp.Scheduler.Quartz.Database.Model;
using FcmSharp.Scheduler.Quartz.Quartz.Jobs;
using Quartz;

namespace FcmSharp.Scheduler.Quartz.Services
{
    public interface ISchedulerService
    {
        Task<Message> ScheduleMessageAsync(Message message, CancellationToken cancellationToken);
    }

    public class SchedulerService : ISchedulerService
    {
        private readonly IScheduler scheduler;

        public SchedulerService(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public async Task<Message> ScheduleMessageAsync(Message message, CancellationToken cancellationToken)
        {
            await SaveJob(message, cancellationToken);

            IJobDetail job = JobBuilder.Create<ProcessMessageJob>()
                .WithIdentity(Guid.NewGuid().ToString())
                .UsingJobData(ProcessMessageJob.JobDataKey, message.Id)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString())
                .StartAt(message.ScheduledTime)
                .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);

            return message;
        }

        private Task SaveJob(Message message, CancellationToken cancellationToken)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Messages.Add(message);

                return context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
