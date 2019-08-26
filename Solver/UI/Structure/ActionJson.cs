using Newtonsoft.Json;

namespace Structure
{
    public class ActionJson
    {
        [JsonProperty("agentID")]
        public int AgentID { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("dx")]
        public int Dx { get; set; }

        [JsonProperty("dy")]
        public int Dy { get; set; }
    }
}
