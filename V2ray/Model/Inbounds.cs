using Newtonsoft.Json;

namespace V2ray.Model
{
    public class ClientConfig
    {
        public dynamic log { get; set; }

        public List<Inbound> inbounds { get; set; }

        public dynamic outbounds { get; set; }

        public dynamic dns { get; set; }

        public dynamic routing { get; set; }

        public dynamic transport { get; set; }
    }

    public class Inbound
    {
        public int port { get; set; }

        public string protocol { get; set; }

        public Settings settings { get; set; }

        public dynamic streamSettings { get; set; }

        public dynamic sniffing { get; set; }
    }

    public class Settings
    {
        [JsonProperty("clients")]
        public List<Client> Clients { get; set; }
    }

    public class Client
    {
        public string id { get; set; }

        public int level { get; set; }

        public int alterId { get; set; }

        public string username { get; set; } = Guid.NewGuid().ToString("N");

        public string createDate { get; set; } = DateTime.Now.ToString();

        public int daysLimit { get; set; } = -1;

        public int trafficLimit { get; set; } = -1;

        public int deviceLimit { get; set; } = -1;
    }
}