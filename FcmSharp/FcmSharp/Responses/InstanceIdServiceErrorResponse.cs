using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class InstanceIdServiceErrorResponse
    {
        [JsonProperty("error")]
        [JsonConverter(typeof())]
        public IidErrorCodeEnum Error { get; set; }
    }
}
