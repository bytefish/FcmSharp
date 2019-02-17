using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace FcmSharp.Scheduler.Quartz.Web.Contracts
{
    public class Message
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusEnum Status { get; set; }

        [JsonProperty("scheduledTime")]
        public DateTime ScheduledTime { get; set; }
    }
}
