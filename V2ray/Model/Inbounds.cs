using Newtonsoft.Json;

namespace V2ray.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ClientConfig
    {
        public Log log { get; set; }

        public List<Inbound> inbounds { get; set; }

        public List<Outbound> outbounds { get; set; }

        public Dns dns { get; set; }

        public Routing routing { get; set; }

        public Transport transport { get; set; }
    }

    public class Dns
    {
        public List<string> servers { get; set; }
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

    public class KcpSettings
    {
        public int uplinkCapacity { get; set; }

        public int downlinkCapacity { get; set; }

        public bool congestion { get; set; }
    }

    public class Log
    {
        public string access { get; set; }

        public string error { get; set; }

        public string loglevel { get; set; }
    }

    public class Outbound
    {
        public string protocol { get; set; }

        public dynamic settings { get; set; }

        public string tag { get; set; }
    }

    public class Routing
    {
        public string domainStrategy { get; set; }

        public List<dynamic> rules { get; set; }
    }

    public class Rule
    {
        public string type { get; set; }

        public List<string> ip { get; set; }

        public string outboundTag { get; set; }
    }

    public class Transport
    {
        public KcpSettings kcpSettings { get; set; }
    }

    public class Client
    {
        public string id { get; set; }

        public int level { get; set; }

        public int alterId { get; set; }

        public string username { get; set; }

        public string createDate { get; set; }

        public int daysLimit { get; set; }

        public int trafficLimit { get; set; }

        public int deviceLimit { get; set; }
    }
}
