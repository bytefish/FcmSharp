using Newtonsoft.Json;

namespace FcmSharp.Responses
{
    public class TopicMessageResponseError
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
