using Newtonsoft.Json;

namespace V2ray.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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
