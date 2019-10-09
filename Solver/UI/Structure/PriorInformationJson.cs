using Newtonsoft.Json;
using System.IO;

namespace UI.Structure
{
    public class PriorInformationJson
    {
        [JsonProperty("id")]  //試合のID
        public int ID { get; set; }

        [JsonProperty("intervalMillis")]  //試合のターンとターンの間の時間(ミリ秒)
        public int IntervalMillis { get; set; }

        [JsonProperty("matchTo")]  //対戦相手の名前
        public string MatchTo { get; set; }

        [JsonProperty("teamID")]  //試合における自分のteamID
        public int TeamID { get; set; }

        [JsonProperty("turnMillis")]  //試合の１ターンあたりの時間(ミリ秒)
        public int TurnMillis { get; set; }

        [JsonProperty("turns")]  //試合のターン数
        public int Turns { get; set; }
        
        public PriorInformationJson(string str)
        {
            var PriorInfo = JsonConvert.DeserializeObject<PriorInformationJson>(str);

            ID = PriorInfo.ID;
            IntervalMillis = PriorInfo.IntervalMillis;
            MatchTo = PriorInfo.MatchTo;
            TeamID = PriorInfo.TeamID;
            TurnMillis = PriorInfo.TurnMillis;
            Turns = PriorInfo.Turns;
        }

        public static PriorInformationJson LoadFromJsonFile(string FileName)
        {
            var PriorInfo = new PriorInformationJson();
            using (var sr = new StreamReader(FileName))
            {
                string JsonStr = sr.ReadToEnd();
                PriorInfo = JsonConvert.DeserializeObject<PriorInformationJson>(JsonStr);
            }
            
            return PriorInfo;
        }

        public new string ToString => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
