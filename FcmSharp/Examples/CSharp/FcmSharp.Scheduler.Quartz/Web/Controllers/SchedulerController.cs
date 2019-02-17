// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Quartz.Services;
using FcmSharp.Scheduler.Quartz.Web.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FcmSharp.Scheduler.Quartz.Web.Controllers
{
    [Controller]
    [Route("scheduler")]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerService schedulerService;

        public SchedulerController(ISchedulerService schedulerService)
        {
            this.schedulerService = schedulerService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Message message, CancellationToken cancellationToken)
        {
            // Convert into the Database Representation:
            var target = Convert(message);

            // Save and Schedule:
            var result = await schedulerService.ScheduleMessageAsync(target, cancellationToken);

            return Ok(result);
        }

        private static Database.Model.Message Convert(Web.Contracts.Message source)
        {
            if(source == null)
            {
                return null;
            }

            return new Database.Model.Message
            {
                Id = source.Id,
                Topic = source.Topic,
                Title = source.Title,
                Body = source.Body,
                ScheduledTime = source.ScheduledTime,
                Status = Convert(source.Status)
            };
        }

        private static Database.Model.StatusEnum Convert(Web.Contracts.StatusEnum source)
        {
            switch(source)
            {
                case StatusEnum.Scheduled:
                    return Database.Model.StatusEnum.Scheduled;
                case StatusEnum.Finished:
                    return Database.Model.StatusEnum.Finished;
                case StatusEnum.Failed:
                    return Database.Model.StatusEnum.Failed;
                default:
                    throw new ArgumentException($"Unknown Source StatusEnum {source}");
            }
        }

        private static Web.Contracts.Message Convert(Database.Model.Message source)
        {
            if (source == null)
            {
                return null;
            }

            return new Web.Contracts.Message
            {
                Id = source.Id,
                Topic = source.Topic,
                Title = source.Title,
                Body = source.Body,
                ScheduledTime = source.ScheduledTime,
                Status = Convert(source.Status)
            };
        }

        private static Web.Contracts.StatusEnum Convert(Database.Model.StatusEnum source)
        {
            switch (source)
            {
                case Database.Model.StatusEnum.Scheduled:
                    return StatusEnum.Scheduled;
                case Database.Model.StatusEnum.Finished:
                    return StatusEnum.Finished;
                case Database.Model.StatusEnum.Failed:
                    return StatusEnum.Failed;
                default:
                    throw new ArgumentException($"Unknown Source StatusEnum {source}");
            }
        }
    }
}
