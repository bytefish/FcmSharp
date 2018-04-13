// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Model.Options;
using FcmSharp.Model.Topics;
using FcmSharp.Requests.Notification;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Topics
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TopicMulticastMessage : FcmMessage
    {
        [JsonProperty("condition")]
        public readonly string Condition;

        public TopicMulticastMessage(FcmMessageOptions options, TopicList topicList, NotificationPayload notification)
            : base(options, notification)
        {
            Condition = topicList.GetTopicsCondition();
        }

        public TopicMulticastMessage(FcmMessageOptions options, string condition, NotificationPayload notification)
            : base(options, notification)
        {
            Condition = condition;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TopicMulticastMessage<TPayload> : TopicMulticastMessage
    {
        [JsonProperty("data")]
        public readonly TPayload Data;

        public TopicMulticastMessage(FcmMessageOptions options, TopicList topicList, TPayload data)
            : this(options, topicList, data, null)
        {
        }

        public TopicMulticastMessage(FcmMessageOptions options, string condition, TPayload data)
            : this(options, condition, data, null)
        {
        }

        public TopicMulticastMessage(FcmMessageOptions options, TopicList topicList, TPayload data, NotificationPayload notification)
            : base(options, topicList, notification)
        {
            Data = data;
        }

        public TopicMulticastMessage(FcmMessageOptions options, string condition, TPayload data, NotificationPayload notification)
            : base(options, condition, notification)
        {
            Data = data;
        }
    }
}