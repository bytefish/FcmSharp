// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class ApnsConfig
    {
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }
        
        [JsonProperty("payload")]
        public ApnsConfigPayload Payload { get; set; }
    }
}
