using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FcmSharp.Requests.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FcmSharp.Requests
{
    public class AndroidConfig
    {
        [JsonProperty("collapse_key")]
        public string CollapseKey { get; set; }

        [JsonProperty("priority")]
        [JsonConverter(typeof(AndroidMessagePriorityEnumConverter))]
        public AndroidMessagePriorityEnum Priority { get; set; }

        [JsonProperty("ttl")]
        [JsonConverter(typeof(DurationFormatConverter))]
        public TimeSpan? TimeToLive { get; set; }

        [JsonProperty("restricted_package_name")]
        public string RestrictedPackageName { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, string> data { get; set; }

        [JsonProperty("notification")]
        public AndroidNotification Notification { get; set; }
    }
}
