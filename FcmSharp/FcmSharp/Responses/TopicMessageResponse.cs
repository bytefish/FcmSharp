// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FcmSharp.Responses
{
    public class TopicMessageResponse
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; }

        [JsonProperty("error")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode? ErrorCode { get; set; }

        public override string ToString()
        {
            return string.Format("TopicMessageResponse (MessageId = {0}, ErrorCode = {1})", MessageId, ErrorCode);
        }
    }
}