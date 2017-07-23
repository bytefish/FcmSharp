// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Model.Options;
using FcmSharp.Model.Topics;
using FcmSharp.Requests.Notification;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Topics
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TopicUnicastMessage : FcmMessage
    {
        [JsonProperty("to")]
        public string To { get; set; }

        public TopicUnicastMessage(FcmMessageOptions options, Topic topic, NotificationPayload notification)
            : base(options, notification)

        {
            if (topic == null)
            {
                throw new ArgumentNullException("topic");
            }
            To = topic.getTopicPath();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TopicUnicastMessage<TPayload> : TopicUnicastMessage
    {
        [JsonProperty("data")]
        public readonly TPayload Data;

        public TopicUnicastMessage(FcmMessageOptions options, Topic topic, TPayload data)
            : this(options, topic, data, null)
        {
        }

        public TopicUnicastMessage(FcmMessageOptions options, Topic topic, TPayload data, NotificationPayload notification)
            : base(options, topic, notification)

        {
            Data = data;
        }
    }
}
