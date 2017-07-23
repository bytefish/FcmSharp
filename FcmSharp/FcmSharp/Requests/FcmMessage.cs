// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Model.Options;
using FcmSharp.Requests.Converters;
using FcmSharp.Requests.Notification;
using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    [JsonObject()]
    public abstract class FcmMessage
    {
        [JsonProperty("notification")]
        public readonly NotificationPayload Notification;

        [JsonProperty("collapse_key")]
        public readonly string CollapseKey;

        [JsonProperty("priority")]
        [JsonConverter(typeof(PriorityEnumConverter))]
        public readonly PriorityEnum? Priority;

        [JsonProperty("content_available")]
        public readonly bool? ContentAvailable;

        [JsonProperty("delay_while_idle")]
        public readonly bool? DelayWhileIdle;

        [JsonProperty("time_to_live")]
        public readonly int? TimeToLive;

        [JsonProperty("restricted_package_name")]
        public readonly string RestrictedPackageName;

        [JsonProperty("dry_run")]
        public readonly bool? DryRun;

        public FcmMessage(FcmMessageOptions options, NotificationPayload notification)
        {
            Notification = notification;
            CollapseKey = options.CollapseKey;
            Priority = options.PriorityEnum;
            ContentAvailable = options.ContentAvailable;
            DelayWhileIdle = options.DelayWhileIdle;
            TimeToLive = options.TimeToLive;
            DryRun = options.DryRun;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class FcmMessage<TPayload> : FcmMessage
    {
        [JsonProperty("data")]
        public readonly TPayload Data;

        public FcmMessage(FcmMessageOptions options, TPayload data, NotificationPayload notification)
            : base(options, notification)
        {
            Data = data;
        }
    }
}
