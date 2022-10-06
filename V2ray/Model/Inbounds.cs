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
        public StreamSettings streamSettings { get; set; }
        public Sniffing sniffing { get; set; }
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
        public Settings settings { get; set; }
        public string tag { get; set; }
    }



    public class Routing
    {
        public string domainStrategy { get; set; }
        public List<Rule> rules { get; set; }
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
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("alterId")]
        public int AlterId { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("createDate")]
        public string CreateDate { get; set; }
    }

    public class Inbounds
    {
        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("streamSettings")]
        public StreamSettings StreamSettings { get; set; }

        [JsonProperty("sniffing")]
        public Sniffing Sniffing { get; set; }
    }

    public class Settings
    {
        [JsonProperty("clients")]
        public List<Client> Clients { get; set; }
    }

    public class Sniffing
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("destOverride")]
        public List<string> DestOverride { get; set; }
    }

    public class StreamSettings
    {
        [JsonProperty("network")]
        public string Network { get; set; }
    }


}
