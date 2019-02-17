// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Scheduler.Quartz.Database.Model
{
    public class Message
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public StatusEnum Status { get; set; }

        public DateTime ScheduledTime { get; set; }
    }
}
