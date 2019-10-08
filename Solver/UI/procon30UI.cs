using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Timers;

namespace procon30UI
{
    public partial class Form1 : Form
    {
        const int Width_length = 1500;//フォームの幅
        const int Height_length = 1000;//フォーㇺの高さ
        const int mass_wid = 33;//マスの幅
        int mass_basic = mass_wid * 4;//空白スペース
        bool GetAPIFlag = true;
        bool GetSeachFlag = true;

        //jsonから取得
        UI.Structure.FieldJson fieldInfo;
        int width = 0;
        int height = 0;
        int[][] points;//ポイント情報
        int[][] tiled;//タイルの情報
        int startedAtUnixTime = 0;//時間管理
        int[,] teams;//チームの情報
        int enemyTeamID = 0;
        int tilePoint = 0;
        int areaPoint = 0;
        int enemyTilePoint = 0;
        int enemyAreaPoint = 0;

        //当日知らされる
        int agentID = 0;
        int enemyagentID = 0;
        int time = 0;
        int turn = 0;//ターン数

        //APIから取得
        int matchID;
        int enemyTeamName;
        int teamID = 0;//jsonからも取得可能？？
        int totalTurn = 10;//試合ごとに変わるその試合の総ターン
        int oneTurnInterval = 0; //試合の1ターンあたりの時間(ms) つかわんくねこれ？
        int turnInterval = 10;//試合のターンとターンの間の時間(ms)

        private void Form1_Load(object sender, EventArgs e)
        {
            Width = Width_length;
            Height = Height_length;
            label11.Text = string.Format("経過時間 {0}秒", time);
        }

        public Form1()
        {
            InitializeComponent();
            fieldInfo = UI.Structure.FieldJson.LoadFromJsonFile("C:\\Users\\Pepper\\Documents\\procon30_json\\F-2.json");

            //jsonからの代入
            width = fieldInfo.Width;
            height = fieldInfo.Height;
            startedAtUnixTime = fieldInfo.StartedAtUnixTime;
            turn = fieldInfo.Turn;
            teamID = fieldInfo.Teams[0].TeamID;
            enemyTeamID = fieldInfo.Teams[1].TeamID;
            tilePoint = fieldInfo.Teams[0].TilePoint;
            areaPoint = fieldInfo.Teams[0].AreaPoint;
            enemyTilePoint = fieldInfo.Teams[1].TilePoint;
            areaPoint = fieldInfo.Teams[1].AreaPoint;

            label1.Text = string.Format("縦 {0}マス", height);
            label2.Text = string.Format("横 {0}マス", width);
            label3.Text = string.Format("{0} ターン目", turn);
            label29.Text = string.Format("開始時のunixtime : {0}", startedAtUnixTime);
            label4.Text = string.Format("タイルポイント       {0}", tilePoint);
            label5.Text = string.Format("エリアポイント        {0}", areaPoint);
            label6.Text = string.Format("敵 タイルポイント  {0}", enemyTilePoint);
            label7.Text = string.Format("敵 エリアポイント   {0}", enemyAreaPoint);

            points = fieldInfo.Points;
            tiled = fieldInfo.Tiled;
            teams = new int[height,width];

            for (int i = 0; i < fieldInfo.Teams[0].Agents.Length; i++)//teamsの代入
            {
                teams[fieldInfo.Teams[0].Agents[i].X - 1, fieldInfo.Teams[0].Agents[i].Y - 1] = fieldInfo.Teams[0].Agents[i].AgentID;
                teams[fieldInfo.Teams[1].Agents[i].X - 1, fieldInfo.Teams[1].Agents[i].Y - 1] = fieldInfo.Teams[1].Agents[i].AgentID;
            }

            //時間管理
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) =>
            {
                time += 1;
                startedAtUnixTime += 1;
            };
            timer.Start();
        }


        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        override protected void OnPaint(PaintEventArgs e)
        {
            this.Left = 10;
            this.Top = 30;

            try
            {
                int remainingturn = totalTurn - turn;
                label9.Text = string.Format("総ターン    {0}ターン", totalTurn);
                label8.Text = string.Format("残り         {0}ターン", remainingturn.ToString());
                label10.Text = string.Format("{0}秒以内に送信", turnInterval);



                if (turnInterval - 3 == time)
                {
                    //一番点数の高いところにいくやつをここに書く
                    //highestScore();
                }

                //プログラムの終了
                if (turn > totalTurn + 3)//念のために+3ほどしておく
                {
                    Environment.Exit(0x8020);
                }
            }

            catch (Exception)
            {

            }

            Graphics g = e.Graphics;
            //grph.DrawLine (ペン，始点x座標， 始点y座標，終点x座標， 終点y座標)
            for (int n = 0; n <= width; n++)
            {
                g.DrawLine(Pens.Black, n * mass_wid + mass_basic, mass_basic, n * mass_wid + mass_basic, mass_wid * height + mass_basic);// y 軸方向の線
            }

            for (int i = 0; i <= height; i++)
            {
                g.DrawLine(Pens.Black, mass_basic, i * mass_wid + mass_basic, mass_wid * width + mass_basic, i * mass_wid + mass_basic);// x軸方向の線 
            }

            if (turn > 0)
            {
                //履歴の入力
                label12.Text = string.Format("行動履歴");
                label13.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", fieldInfo.Actions[0], agentID, "move", 1, 1, 1, 1);
                label14.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label15.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label16.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label17.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label18.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label19.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label20.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label21.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label22.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label23.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label24.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label25.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label26.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label27.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
                label28.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);

                //移動後のマスに色を付ける
                foreach (UI.Structure.FieldJson.Action action in fieldInfo.Actions)
                {
                    foreach(UI.Structure.FieldJson.Team.Agent agent in fieldInfo.Teams[0].Agents)
                    {
                        if (!action.Apply.Equals("move")) break;
                        if (action.AgentID == agent.AgentID)
                        {
                            if (fieldInfo.Teams[0].TeamID == 1)//TODO あとで変更の可能性あり
                            {
                                Rectangle rect = new Rectangle((agent.X + action.Dx -1) * mass_wid + mass_basic + 1, (agent.Y + action.Dy -1) * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                                Graphics a = CreateGraphics();
                                g.FillRectangle(Brushes.HotPink, rect);
                            }

                            else
                            {
                                Rectangle rect = new Rectangle((agent.X + action.Dx - 1) * mass_wid + mass_basic + 1, (agent.Y + action.Dy - 1) * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                                Graphics a = CreateGraphics();
                                g.FillRectangle(Brushes.CornflowerBlue, rect);
                            }

                            break;
                        }
                    }
                }
            }

            //agentの場所に色を付ける
            foreach (UI.Structure.FieldJson.Team team in fieldInfo.Teams)
            {
                Brush brush;
                if (team.TeamID == 1)//TODO あとで変更の可能性あり
                {
                    brush = Brushes.DeepPink;
                }
                else
                {
                    brush = Brushes.RoyalBlue;
                }

                foreach (UI.Structure.FieldJson.Team.Agent agent in team.Agents)
                {
                    Rectangle rect = new Rectangle((agent.X - 1) * mass_wid + mass_basic + 1, (agent.Y - 1) * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                    Graphics a = CreateGraphics();
                    g.FillRectangle(brush, rect);
                }
            }

            //点数をフィールドに入れる
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Graphics a = e.Graphics;
                    Font ft = new Font("MS Serif", 15);
                    PointF pt = new PointF(j * mass_wid + mass_basic + 2, i * mass_wid + mass_basic + 6);
                    a.DrawString(points[i][j].ToString(), ft, Brushes.Black, pt);
                }
            }

            //int[] agent;
            //agent = new int[agentOfenemies];
            // public void highestScore()
            // {
            //     if()
            //     {            
            //           for(int n =0;n<agent[agentOfenemies];n++)
            //           {
            //               for(int i =0;i<height;i++)
            //               {
            //                   for(int j =0;j<width;j++)
            //                   {       
            //                       if(teams[i,j] == agentID[0] || teams[i,j] == agentID[1])
            //                       {
            //                           int[]  mawari;
            //                           mawari = new int[8];
            //                           mawari[0] = points[i-1,j-1]; mawari[1] = points[i-1,j]; mawari[2] = points[i-1,j+1];
            //                           mawari[3] = points[i,j-1];                               mawari[4] = points[i,j+1];
            //                           mawari[5] = points[i+1,j-1]; mawari[6] = points[i+1,j]; mawari[7] = points[i+1,j+1];
            //                           Array.Sort(mawari);
            //                       //mawari[8]; //これが一番大きい
            //                       }
            //                   }
            //               }
            //           }
            //     }
            //周りが自分のタイルか否か？
            //↓
            //周りで最も点数が高いものはどれか？
            // }
        }

        public void SearchStart()
        {
            time = 0;
            //探索の開始
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("notepad.exe");//探索.exe
            /*
            while()//探索の終了待ち
            {
                if()//時間のチェック過ぎれば周りで最もポイントの高い方へ
                {
                    
                }
            }
            */
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SearchStart();
        }

        private void tickTimer(object sender, EventArgs e)
        {
            label11.Text = string.Format("経過時間 {0}秒", time);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //事前情報取得するまでwhileを回すかんじ
            /*
            while (GetAPIFlag)
            {
                if (事前情報が入ったら)
                {
                    GetAPIFlag = false;
                }
            }*/
        }
    }
}