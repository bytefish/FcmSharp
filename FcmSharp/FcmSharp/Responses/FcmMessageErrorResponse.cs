// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class FcmMessageErrorResponse
    {
        [JsonProperty("error")]
        public FcmMessageError Error { get; set; }
    }

    public class FcmMessageError
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public List<FcmMessageErrorItem> Errors { get; set; }

        /// <summary>
        /// Error reason 
        /// </summary>
        /// <example>
        /// e.g. "NOT_FOUND" when the token the message was sent to no longer exists
        /// </example>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class FcmMessageErrorItem
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
