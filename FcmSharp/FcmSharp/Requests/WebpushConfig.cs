// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class WebpushConfig
    {
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }

        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }

        [JsonProperty("notification")]
        public WebpushNotification Notification { get; set; }

        [JsonProperty("fcm_options")]
        public WebpushFcmOptions FcmOptions { get; set; }
    }
}
