// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class TopicManagementRequest
    {
        [JsonProperty("to")]
        public string Topic { get; set; }

        [JsonProperty("registration_tokens")]
        public string[] RegistrationTokens { get; set; }
    }
}
