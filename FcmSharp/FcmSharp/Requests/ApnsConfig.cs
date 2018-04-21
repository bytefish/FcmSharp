using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class ApnsConfig
    {
        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("aps")]
        public Aps Aps { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> CustomData { get; set; }
    }
}
