namespace FcmSharp.Http.Builder
{
    public class UrlSegment
    {
        public readonly string name;
        public readonly string value;

        public UrlSegment(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
