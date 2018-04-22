namespace FcmSharp.Settings
{
    public class FcmClientSettings
    {
        public FcmClientSettings(string projectId)
        {
            IidUrl = "https://iid.googleapis.com";
            FcmUrl = $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send";
        }
        
        public string IidUrl { get; private set; }

        public string FcmUrl { get; private set; }

        public string Credentials { get; private set; }
    }
}