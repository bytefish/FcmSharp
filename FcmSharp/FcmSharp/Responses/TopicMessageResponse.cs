// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Responses.Converters;
using Newtonsoft.Json;

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
            TooManyTopics,
            PermissionDenied
        }
        
        public class ResultItem
        {
            [JsonProperty("error")]
            [JsonConverter(typeof(TopicErrorEnumConverter))]
            public Error? ErrorCode { get; set; }
        }

        [JsonProperty("results")]
        public ResultItem[] Results { get; set; }
    }
}