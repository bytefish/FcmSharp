// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FcmSharp.Responses
{
    public class FcmMessageResultItem
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("registration_id")]
        public string CanonicalRegistrationId { get; set; }

        [JsonProperty("error")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode? Error { get; set; }

        public override string ToString()
        {
            return string.Format("FcmMessageResultItem (MessageId = {0}, CanonicalRegistrationId = {1}, Error = {2})", MessageId, CanonicalRegistrationId, Error);
        }
    }
}