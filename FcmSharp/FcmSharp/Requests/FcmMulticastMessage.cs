// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FcmSharp.Model.Options;
using FcmSharp.Requests.Notification;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FcmMulticastMessage : FcmMessage
    {
        [JsonProperty("registration_ids")]
        public readonly IList<string> RegistrationIds;

        public FcmMulticastMessage(FcmMessageOptions options, IList<string> registrationIds, NotificationPayload notification)
            : base(options, notification)
        {
            RegistrationIds = registrationIds;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FcmMulticastMessage<TPayload> : FcmMulticastMessage
    {
        [JsonProperty("data")]
        public readonly TPayload Data;

        public FcmMulticastMessage(FcmMessageOptions options, IList<string> registrationIds, TPayload data)
            : base(options, registrationIds, null)
        {
            Data = data;
        }

        public FcmMulticastMessage(FcmMessageOptions options, IList<string> registrationIds, TPayload data, NotificationPayload notification)
            : base(options, registrationIds, notification)
        {
            Data = data;
        }
    }
}
