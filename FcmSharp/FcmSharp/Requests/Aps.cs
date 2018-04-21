using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FcmSharp.Requests.Converters;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class Aps
    {
        [JsonProperty("alert")]
        public string Alert { get; set; }

        [JsonProperty("badge")]
        public string Badge { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("content-available")]
        [JsonConverter(typeof(BoolToIntConverter))]
        public bool ContentAvailable { get; set; }

        [JsonProperty("mutable-content")]
        [JsonConverter(typeof(BoolToIntConverter))]
        public bool MutableContent { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("thread-id")]
        public string ThreadId { get; set; }

        public Dictionary<string, object> CustomData { get; set; }
    }
}
