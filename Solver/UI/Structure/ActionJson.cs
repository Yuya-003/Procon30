using Newtonsoft.Json;
using System.IO;

namespace UI.Structure
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

        public ActionJson(){}

        public ActionJson(string str)
        {
            ActionJson action = JsonConvert.DeserializeObject<ActionJson>(str);

            AgentID = action.AgentID;
            Type = action.Type;
            Dx = action.Dx;
            Dy = action.Dy;
        }

        public static ActionJson LoadFromJsonFile(string FileName)
        {
            var action = new ActionJson();
            using (var sr = new StreamReader(FileName))
            {
                string JsonStr = sr.ReadToEnd();
                action = JsonConvert.DeserializeObject<ActionJson>(JsonStr);
            }
            
            return action;
        }

        public new string ToString => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
