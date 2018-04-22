// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Responses.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FcmSharp.Responses
{
    public class TopicManagementResponse
    {
        public enum Error
        {
            Unknown,
            InvalidArgument,
            NotFound,
            Internal,
            TooManyTopics
        }

        public class ResultItem
        {
            [JsonProperty("error")]
            [JsonConverter(typeof(TopicErrorEnumConverter))]
            public Error? ErrorCode { get; set; }

            public override string ToString()
            {
                return string.Format("TopicMessageResponse (MessageId = {0}, ErrorCode = {1})", MessageId, ErrorCode);
            }
        }
    }
}