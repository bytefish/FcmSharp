using System;
using System.Collections.Generic;
using System.Text;

namespace FcmSharp.Scheduler.Database.Model
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
