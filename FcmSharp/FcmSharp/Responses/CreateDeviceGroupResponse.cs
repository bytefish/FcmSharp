// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class CreateDeviceGroupMessageResponse
    {
        [JsonProperty("notification_key")]
        public string NotificationKey { get; set; }
    }
}
