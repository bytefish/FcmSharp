// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FcmSharp.Requests.Converters;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class Aps
    {
        [JsonProperty("alert")]
        public ApsAlert Alert { get; set; }

        [JsonProperty("badge")]
        public int? Badge { get; set; }

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

        [JsonExtensionData]
        public IDictionary<string, object> CustomData { get; set; }
    }
}
