// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Quartz.Services;
using FcmSharp.Scheduler.Quartz.Web.Contracts;
using FcmSharp.Scheduler.Quartz.Web.Converters;
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
            var target = MessageConverter.Convert(message);

            // Save and Schedule:
            var result = await schedulerService.ScheduleMessageAsync(target, cancellationToken);

            return Ok(result);
        }
    }
}
