// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class FcmBatchResponse
    {
        [JsonProperty("responses")]
        public FcmSendResponse[] Responses { get; set; }

        [JsonProperty("failureCount")]
        public int FailureCount { get; set; }

        [JsonProperty("successCount")]
        public int SuccessCount { get; set; }
    }
}
