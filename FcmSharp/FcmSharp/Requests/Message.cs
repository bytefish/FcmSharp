// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class Message
    {
        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }

        [JsonProperty("notification")]
        public Notification Notification { get; set; }

        [JsonProperty("android")]
        public AndroidConfig AndroidConfig { get; set; }

        [JsonProperty("webpush")]
        public WebpushConfig WebpushConfig { get; set; }

        [JsonProperty("apns")]
        public ApnsConfig ApnsConfig { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }
    }
}