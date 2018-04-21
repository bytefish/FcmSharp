using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Topics
{
    public class AndroidNotification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("click_action")]
        public string ClickAction { get; set; }

        [JsonProperty("body_loc_key")]
        public string BodyLocKey { get; set; }

        [JsonProperty("body_loc_args")]
        public string[] BodyLocArgs { get; set; }

        [JsonProperty("title_loc_key")]
        public string TitleLocKey { get; set; }

        [JsonProperty("title_loc_args")]
        public string[] TitleLocArgs { get; set; }


    }
}
