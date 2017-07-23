// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class FcmMessageResponse
    {
        [JsonProperty("multicast_id")]
        public long MulticastId { get; set; }

        [JsonProperty("success")]
        public int NumberOfSuccess { get; set; }

        [JsonProperty("failure")]
        public int NumberOfFailure { get; set; }

        [JsonProperty("canoncial_ids")]
        public int NumberOfCanonicalIds { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("results")]
        public List<FcmMessageResultItem> Results { get; set; }

        public override string ToString()
        {
            return string.Format("FcmMulticastMessageResponse (MulticastId = {0}, NumberOfSuccess = {1}, NumberOfFailure = {2}, NumberOfCanonicalIds = {3}, MessageId = {4}, Results = [{5}])",
                MulticastId, NumberOfSuccess, NumberOfFailure, NumberOfCanonicalIds, MessageId, string.Join(", ", Results));
        }
    }
}