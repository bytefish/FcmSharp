// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using FcmSharp.Scheduler.Quartz.Services;
using Quartz;

namespace FcmSharp.Scheduler.Quartz.Quartz.Jobs
{
    public class ProcessMessageJob : IJob
    {
        public static readonly string JobDataKey = "MESSAGE_ID";

        private readonly IMessagingService messagingService;

        public ProcessMessageJob(IMessagingService messagingService)
        {
            this.messagingService = messagingService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;
            var messageId = GetMessageId(context);

            await messagingService.SendScheduledMessageAsync(messageId, cancellationToken);
        }

        private int GetMessageId(IJobExecutionContext context)
        {
            JobDataMap jobDataMap = context.JobDetail.JobDataMap;

            return jobDataMap.GetIntValue(JobDataKey);
        }
    }
}
