using Newtonsoft.Json;

namespace Structure
{
    public class FieldJson
    {
        public class Team
        {
            public class Agent
            {
                [JsonProperty("agentID")]
                public int AgentID { get; set; }

                [JsonProperty("x")]
                public int X { get; set; }

                [JsonProperty("y")]
                public int Y { get; set; }
            }

            [JsonProperty("teamID")]
            public int TeamID { get; set; }

            [JsonProperty("tilePoint")]
            public int TilePoint { get; set; }

            [JsonProperty("areaPoint")]
            public int AreaPoint { get; set; }

            [JsonProperty("agents")]
            public Agent[] Agents { get; set; }
        }

        public class Action
        {
            [JsonProperty("agentID")]
            public int AgentID { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("dx")]
            public int Dx { get; set; }

            [JsonProperty("dy")]
            public int Dy { get; set; }

            [JsonProperty("turn")]
            public int Turn { get; set; }

            [JsonProperty("apply")]
            public int Apply { get; set; }
        }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("points")]
        public int[][] Points { get; set; }

        [JsonProperty("startedAtUnixTime")]
        public int StartedAtUnixTime { get; set; }

        [JsonProperty("turn")]
        public int Turn { get; set; }

        [JsonProperty("teams")]
        public Team[] Teams { get; set; }

        [JsonProperty("actions")]
        public Action[] Actions { get; set; }
    }
}

