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
        
        [JsonProperty("android_channel_id")]
        public string AndroidChannelId { get; set; }

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
        
        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

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
            // Build Comma Separated Argument List:
            var bodyLocArgList = BodyLocArgs != null ? string.Join(", ", BodyLocArgs) : string.Empty;
            var titleLocArgList = TitleLocArgs != null ? string.Join(", ", TitleLocArgs) : string.Empty;

            return $"NotificationPayload (Sound = {Sound}, Badge = {Badge}, Tag = {Tag}, Color = {Color}, ClickAction = {ClickAction}, BodyLocKey = {BodyLocKey}, BodyLocArgs = [{bodyLocArgList}], TitleLocKey = {TitleLocKey}, TitleLocArgs = [{titleLocArgList}], AndroidChannelId = {AndroidChannelId})";
        }
    }
}
