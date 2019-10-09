using Newtonsoft.Json;
using System.IO;

namespace UI.Structure
{
    public class ActionsJson{
        public class Action
        {
            [JsonProperty("agentID")]  //エージェントID
            public int AgentID { get; set; }

            [JsonProperty("type")]  //行動の種類("move", "remove", "stay")
            public string Type { get; set; }

            [JsonProperty("dx")]  //行動のx方向の向き
            public int Dx { get; set; }

            [JsonProperty("dy")]  //行動のy方向の向き
            public int Dy { get; set; }
        }

        [JsonProperty("actions")]
        public Action[] Actions { get; set; }

        public ActionsJson(){}

        public ActionsJson(string str)
        {
            ActionsJson actions = JsonConvert.DeserializeObject<ActionsJson>(str);

            Actions = actions.Actions;
        }

        public static ActionsJson LoadFromJsonFile(string FileName)
        {
            var actions = new ActionsJson();
            using (var sr = new StreamReader(FileName))
            {
                string JsonStr = sr.ReadToEnd();
                actions = JsonConvert.DeserializeObject<ActionsJson>(JsonStr);
            }
            
            return actions;
        }

        public new string ToString => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
