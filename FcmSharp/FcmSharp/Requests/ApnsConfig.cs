// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
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
