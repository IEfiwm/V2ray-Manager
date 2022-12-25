namespace V2ray.Model
{
    public class Config
    {
        public string Name { get; set; } = "Server Name";

        public List<string> HostNames { get; set; }

        public string Port { get; set; }

        public string Type { get; set; }

        public string Net { get; set; }

        public string Level { get; set; }

        public string Security { get; set; }

        public string Tls { get; set; }

        public string Sni { get; set; }

        public string Path { get; set; }

        public string Host { get; set; }
    }
}