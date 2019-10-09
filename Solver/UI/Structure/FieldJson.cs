using Newtonsoft.Json;
using System.IO;

namespace UI.Structure
{
    public class FieldJson
    {
        public class Team
        {
            public class Agent
            {
                [JsonProperty("agentID")]  //エージェントID
                public int AgentID { get; set; }

                [JsonProperty("x")]  //x座標
                public int X { get; set; }

                [JsonProperty("y")]  //y座標
                public int Y { get; set; }
            }

            [JsonProperty("teamID")]  //チームID
            public int TeamID { get; set; }

            [JsonProperty("tilePoint")]  //タイルポイント
            public int TilePoint { get; set; }

            [JsonProperty("areaPoint")]  //領域ポイント
            public int AreaPoint { get; set; }

            [JsonProperty("agents")]  //各エージェント状況
            public Agent[] Agents { get; set; }
        }

        public class Action
        {
            [JsonProperty("agentID")]  //エージェントID
            public int AgentID { get; set; }

            [JsonProperty("type")]  //行動の種類
            public string Type { get; set; }

            [JsonProperty("dx")]  //行動のx方向の向き
            public int Dx { get; set; }

            [JsonProperty("dy")]  //行動のy方向の向き
            public int Dy { get; set; }

            [JsonProperty("turn")]  //行動ターン
            public int Turn { get; set; }

            [JsonProperty("apply")]  //行動の適応状況
            public int Apply { get; set; }
        }

        [JsonProperty("width")]  //フィールド横幅
        public int Width { get; set; }

        [JsonProperty("height")]  //フィールド縦幅
        public int Height { get; set; }

        [JsonProperty("points")]  //各マスの点数
        public int[][] Points { get; set; }

        [JsonProperty("startedAtUnixTime")]  //試合が始まったUnix時間
        public int StartedAtUnixTime { get; set; }

        [JsonProperty("turn")]  //ターン
        public int Turn { get; set; }

        [JsonProperty("tiled")]  //タイル配置状況
        public int[][] Tiled { get; set; }

        [JsonProperty("teams")]  //各チーム状況
        public Team[] Teams { get; set; }

        [JsonProperty("actions")]  //各行動履歴
        public Action[] Actions { get; set; }

        public FieldJson(){}

        public FieldJson(string str)
        {
            FieldJson field = JsonConvert.DeserializeObject<FieldJson>(str);

            Width = field.Width;
            Height = field.Height;
            Points = field.Points;
            StartedAtUnixTime = field.StartedAtUnixTime;
            Turn = field.Turn;
            Tiled = field.Tiled;
            Teams = field.Teams;
            Actions = field.Actions;
        }

        public static FieldJson LoadFromJsonFile(string FileName)
        {
            var field = new FieldJson();
            using (var sr = new StreamReader(FileName))
            {
                string JsonStr = sr.ReadToEnd();
                field = JsonConvert.DeserializeObject<FieldJson>(JsonStr);
            }

            return field;
        }

        public new string ToString => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}

