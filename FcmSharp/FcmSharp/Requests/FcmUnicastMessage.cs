// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Model.Options;
using FcmSharp.Requests.Notification;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class FcmUnicastMessage : FcmMessage
    {
        [JsonProperty("to")]
        public readonly string To;

        public FcmUnicastMessage(FcmMessageOptions options, String to, NotificationPayload notification)
            : base(options, notification)
        {
            To = to;
        }
    }

    public class FcmUnicastMessage<TPayload> : FcmUnicastMessage
    {
        [JsonProperty("data")]
        public readonly TPayload Data;

        public FcmUnicastMessage(FcmMessageOptions options, String to, TPayload data, NotificationPayload notification)
            : base(options, to, notification)
        {
            Data = data;
        }
    }
}
