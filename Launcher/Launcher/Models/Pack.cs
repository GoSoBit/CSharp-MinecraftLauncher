namespace Launcher.Models
{
    public class Pack
    {
        public Pack()
        {
        }

        public Pack(string id, string type, string url)
        {
            Id = id;
            Type = type;
            Url = url;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}