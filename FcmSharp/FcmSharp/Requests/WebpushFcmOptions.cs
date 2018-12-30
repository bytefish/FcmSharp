using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class WebpushFcmOptions
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
