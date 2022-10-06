using Newtonsoft.Json;

namespace V2ray.Model
{
    public class User
    {
        [JsonProperty("v")]
        public string V { get; set; } = "";

        [JsonProperty("ps")]
        public string Ps { get; set; } = "";

        [JsonProperty("add")]
        public string Add { get; set; } = "";

        [JsonProperty("port")]
        public int Port { get; set; } = 0;

        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("aid")]
        public string Aid { get; set; } = "0";

        [JsonProperty("net")]
        public string Net { get; set; } = "";

        [JsonProperty("type")]
        public string Type { get; set; } = "";

        [JsonProperty("host")]
        public string Host { get; set; } = "";

        [JsonProperty("path")]
        public string Path { get; set; } = "";

        [JsonProperty("tls")]
        public string Tls { get; set; } = "";
    }
}
