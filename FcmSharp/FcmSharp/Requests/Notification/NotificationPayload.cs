// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Notification
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NotificationPayload
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("badge")]
        public string Badge { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("click_action")]
        public string ClickAction { get; set; }

        [JsonProperty("body_loc_key")]
        public string BodyLocKey { get; set; }

        [JsonProperty("body_loc_args")]
        public IList<string> BodyLocArgs { get; set; }

        [JsonProperty("title_loc_key")]
        public string TitleLocKey { get; set; }

        [JsonProperty("title_loc_args")]
        public IList<string> TitleLocArgs { get; set; }

        public override string ToString()
        {
            return string.Format("NotificationPayload (Sound = {0}, Badge = {1}, Tag = {2}, Color = {3}, ClickAction = {4}, BodyLocKey = {5}, BodyLocArgs = [{6}], TitleLocKey = {7}, TitleLocArgs = [{8}])",
                Title, Body, Icon, Sound, Badge, Tag, Color, ClickAction, BodyLocKey, string.Join(", ", BodyLocArgs), TitleLocKey, string.Join(", ", TitleLocArgs));
        }
    }
}
