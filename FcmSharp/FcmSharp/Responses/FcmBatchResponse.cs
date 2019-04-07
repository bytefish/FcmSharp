// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class FcmBatchResponse
    {
        public FcmSendResponse[] Responses { get; set; }
    }

    public class FcmSendResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("error")]
        public IDictionary<string, object> Error { get; set; }
    }
}
