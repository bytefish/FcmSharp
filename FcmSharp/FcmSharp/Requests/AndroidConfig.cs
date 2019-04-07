// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using FcmSharp.Requests.Converters;
using Newtonsoft.Json;

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
        public IDictionary<string, string> Data { get; set; }

        [JsonProperty("notification")]
        public AndroidNotification Notification { get; set; }
    }
}
