// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class FcmMessage
    {
        [JsonProperty("validate_only")]
        public bool ValidateOnly { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}
