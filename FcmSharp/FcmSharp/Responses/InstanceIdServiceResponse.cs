using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class InstanceIdServiceResponse
    {
        [JsonProperty("results")]
        public Dictionary<string, object>[] Results { get; set; }
    }
}
