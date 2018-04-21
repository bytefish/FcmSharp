using Newtonsoft.Json;

namespace FcmSharp.Requests
{
    public class TopicManagementRequest
    {
        [JsonProperty("to")]
        public string Topic { get; set; }

        [JsonProperty("registration_tokens")]
        public string[] RegistrationTokens { get; set; }
    }
}
