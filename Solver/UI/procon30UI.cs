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
        int width = 0;
        int height = 0;
        int[][] points;//ポイント情報
        int[,] tiled;//タイルの情報
        int startedAtUnixTime = 0;//時間管理

        int[,] teams;//チームの情報


        int enemyTeamID = 6;

        int tilePoint = 9;
        int areaPoint = 0;
        int enemytilePoint = 9;
        int enemyareaPoint = 0;

        //当日知らされる
        int agentID = 9;
        int enemyagentID = 10;


        //agentの数を受け取る
        int agentOfenemies = 6;//2~8

        int time = 0;
        int turn = 0;//ターン数


        //jsonから取得
        //APIから取得

        int matchID;
        int enemyTeamName;
        int teamID = 5;//jsonからも取得可能？？
        int totalTurn = 10;//試合ごとに変わるその試合の総ターン
                           //int oneTurnInterval 試合の1ターンあたりの時間(ms) つかわんくねこれ？
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
            var fieldInfo = UI.Structure.FieldJson.LoadFromJsonFile("C:\\Users\\Pepper\\Documents\\procon30_json\\F-2.json");

            //jsonからの代入
            width = fieldInfo.Width;//フィールドの横マス
            height = fieldInfo.Height;//フィールドの縦マス
            startedAtUnixTime = fieldInfo.StartedAtUnixTime;
            turn = fieldInfo.Turn;

            label1.Text = string.Format("縦 {0}マス", height);
            label2.Text = string.Format("横 {0}マス", width);
            label3.Text = string.Format("{0} ターン目", turn);
            label29.Text = string.Format("開始時のunixtime : {0}", startedAtUnixTime);

            label4.Text = string.Format("タイルポイント       {0}", tilePoint);
            label5.Text = string.Format("エリアポイント        {0}", areaPoint);
            label6.Text = string.Format("敵 タイルポイント  {0}", enemytilePoint);
            label7.Text = string.Format("敵 エリアポイント   {0}", enemyareaPoint);


            int[] agentID = new int[agentOfenemies];//半分より上は敵
            for (int i = 0; i < agentOfenemies; i++)
            {
                //agentID[i] = agentID;

                agentID[0] = 9;
                agentID[1] = 10;
                agentID[2] = 11;
                agentID[3] = 12;
                agentID[4] = 13;
                agentID[5] = 14;
            }





            points = fieldInfo.Points;
            tiled = new int[height, width];

            tiled[0, 0] = 0; tiled[0, 1] = 0; tiled[0, 2] = 0; tiled[0, 3] = 0; tiled[0, 4] = 0; tiled[0, 5] = 0; tiled[0, 6] = 0; tiled[0, 7] = 0; tiled[0, 8] = 0; tiled[0, 9] = 0;
            tiled[1, 0] = 5; tiled[1, 1] = 0; tiled[1, 2] = 0; tiled[1, 3] = 0; tiled[1, 4] = 0; tiled[1, 5] = 0; tiled[1, 6] = 0; tiled[1, 7] = 0; tiled[1, 8] = 0; tiled[1, 9] = 6;
            tiled[2, 0] = 0; tiled[2, 1] = 0; tiled[2, 2] = 0; tiled[2, 3] = 0; tiled[2, 4] = 0; tiled[2, 5] = 0; tiled[2, 6] = 0; tiled[2, 7] = 0; tiled[2, 8] = 0; tiled[2, 9] = 0;
            tiled[3, 0] = 0; tiled[3, 1] = 0; tiled[3, 2] = 0; tiled[3, 3] = 0; tiled[3, 4] = 5; tiled[3, 5] = 0; tiled[3, 6] = 0; tiled[3, 7] = 0; tiled[3, 8] = 0; tiled[3, 9] = 0;
            tiled[4, 0] = 0; tiled[4, 1] = 0; tiled[4, 2] = 0; tiled[4, 3] = 0; tiled[4, 4] = 0; tiled[4, 5] = 0; tiled[4, 6] = 0; tiled[4, 7] = 0; tiled[4, 8] = 0; tiled[4, 9] = 0;
            tiled[5, 0] = 0; tiled[5, 1] = 0; tiled[5, 2] = 0; tiled[5, 3] = 0; tiled[5, 4] = 0; tiled[5, 5] = 0; tiled[5, 6] = 0; tiled[5, 7] = 0; tiled[5, 8] = 0; tiled[5, 9] = 0;
            tiled[6, 0] = 0; tiled[6, 1] = 0; tiled[6, 2] = 0; tiled[6, 3] = 0; tiled[6, 4] = 0; tiled[6, 5] = 6; tiled[6, 6] = 0; tiled[6, 7] = 0; tiled[6, 8] = 0; tiled[6, 9] = 0;
            tiled[7, 0] = 0; tiled[7, 1] = 0; tiled[7, 2] = 0; tiled[7, 3] = 0; tiled[7, 4] = 0; tiled[7, 5] = 0; tiled[7, 6] = 0; tiled[7, 7] = 0; tiled[7, 8] = 0; tiled[7, 9] = 0;
            tiled[8, 0] = 5; tiled[8, 1] = 0; tiled[8, 2] = 0; tiled[8, 3] = 0; tiled[8, 4] = 0; tiled[8, 5] = 0; tiled[8, 6] = 0; tiled[8, 7] = 0; tiled[8, 8] = 0; tiled[8, 9] = 6;
            tiled[9, 0] = 0; tiled[9, 1] = 0; tiled[9, 2] = 0; tiled[9, 3] = 0; tiled[9, 4] = 0; tiled[9, 5] = 0; tiled[9, 6] = 0; tiled[9, 7] = 0; tiled[9, 8] = 0; tiled[9, 9] = 0;

            teams = new int[height, width];

            teams[0, 0] = 0; teams[0, 1] = 0; teams[0, 2] = 0; teams[0, 3] = 0; teams[0, 4] = 0; teams[0, 5] = 0; teams[0, 6] = 0; teams[0, 7] = 0; teams[0, 8] = 0; teams[0, 9] = 0;
            teams[1, 0] = agentID[0]; teams[1, 1] = 0; teams[1, 2] = 0; teams[1, 3] = 0; teams[1, 4] = 0; teams[1, 5] = 0; teams[1, 6] = 0; teams[1, 7] = 0; teams[1, 8] = 0; teams[1, 9] = agentID[1];
            teams[2, 0] = 0; teams[2, 1] = 0; teams[2, 2] = 0; teams[2, 3] = 0; teams[2, 4] = 0; teams[2, 5] = 0; teams[2, 6] = 0; teams[2, 7] = 0; teams[2, 8] = 0; teams[2, 9] = 0;
            teams[3, 0] = 0; teams[3, 1] = 0; teams[3, 2] = 0; teams[3, 3] = 0; teams[3, 4] = agentID[0]; teams[3, 5] = 0; teams[3, 6] = 0; teams[3, 7] = 0; teams[3, 8] = 0; teams[3, 9] = 0;
            teams[4, 0] = 0; teams[4, 1] = 0; teams[4, 2] = 0; teams[4, 3] = 0; teams[4, 4] = 0; teams[4, 5] = 0; teams[4, 6] = 0; teams[4, 7] = 0; teams[4, 8] = 0; teams[4, 9] = 0;
            teams[5, 0] = 0; teams[5, 1] = 0; teams[5, 2] = 0; teams[5, 3] = 0; teams[5, 4] = 0; teams[5, 5] = 0; teams[5, 6] = 0; teams[5, 7] = 0; teams[5, 8] = 0; teams[5, 9] = 0;
            teams[6, 0] = 0; teams[6, 1] = 0; teams[6, 2] = 0; teams[6, 3] = 0; teams[6, 4] = 0; teams[6, 5] = agentID[1]; teams[6, 6] = 0; teams[6, 7] = 0; teams[6, 8] = 0; teams[6, 9] = 0;
            teams[7, 0] = 0; teams[7, 1] = 0; teams[7, 2] = 0; teams[7, 3] = 0; teams[7, 4] = 0; teams[7, 5] = 0; teams[7, 6] = 0; teams[7, 7] = 0; teams[7, 8] = 0; teams[7, 9] = 0;
            teams[8, 0] = agentID[0]; teams[8, 1] = 0; teams[8, 2] = 0; teams[8, 3] = 0; teams[8, 4] = 0; teams[8, 5] = 0; teams[8, 6] = 0; teams[8, 7] = 0; teams[8, 8] = 0; teams[8, 9] = agentID[1];
            teams[9, 0] = 0; teams[9, 1] = 0; teams[9, 2] = 0; teams[9, 3] = 0; teams[9, 4] = 0; teams[9, 5] = 0; teams[9, 6] = 0; teams[9, 7] = 0; teams[9, 8] = 0; teams[9, 9] = 0;

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

                label9.Text = string.Format("総ターン    {0}ターン", totalTurn);
                int remainingturn = totalTurn - turn;

                label8.Text = string.Format("残り         {0}ターン", remainingturn.ToString());

                label10.Text = string.Format("{0}秒以内に送信", turnInterval);

                //履歴の入力
                label12.Text = string.Format("行動履歴");
                label13.Text = string.Format("agentID : {0} Type : {1} dx : {2} dy : {3} Turn : {4} apply : {5}", 1, "move", 1, 1, 1, 1);
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

            for (int n = 0; n <= width; n++)
            {
                //grph.DrawLine (ペン，始点x座標， 始点y座標，終点x座標， 終点y座標)
                g.DrawLine(Pens.Black, n * mass_wid + mass_basic, mass_basic, n * mass_wid + mass_basic, mass_wid * height + mass_basic);// y 軸方向の線
            }

            for (int i = 0; i <= height; i++)
            {
                //grph.DrawLine (ペン，始点x座標， 始点y座標，終点x座標， 終点y座標)
                g.DrawLine(Pens.Black, mass_basic, i * mass_wid + mass_basic, mass_wid * width + mass_basic, i * mass_wid + mass_basic);// x軸方向の線 
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (tiled[i, j] == teamID)
                    {
                        Rectangle rect = new Rectangle(j * mass_wid + mass_basic + 1, i * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                        Graphics a = CreateGraphics();
                        g.FillRectangle(Brushes.HotPink, rect);
                    }

                    if (tiled[i, j] == enemyTeamID)
                    {
                        Rectangle rect = new Rectangle(j * mass_wid + mass_basic + 1, i * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                        Graphics a = CreateGraphics();
                        g.FillRectangle(Brushes.CornflowerBlue, rect);
                    }

                    if (teams[i, j] == agentID)
                    {
                        Rectangle rect = new Rectangle(j * mass_wid + mass_basic + 1, i * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                        Graphics a = CreateGraphics();
                        g.FillRectangle(Brushes.DeepPink, rect);
                    }

                    if (teams[i, j] == enemyagentID)
                    {
                        Rectangle rect = new Rectangle(j * mass_wid + mass_basic + 1, i * mass_wid + mass_basic + 1, mass_wid - 1, mass_wid - 1);
                        Graphics a = CreateGraphics();
                        g.FillRectangle(Brushes.RoyalBlue, rect);
                    }
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

            /*int[] agent;
            agent = new int[agentOfenemies];
             public static void highestScore()
             {
                 if()
                 {            
                       for(int n =0;n<agent[agentOfenemies];n++)
                       {
                           for(int i =0;i<height;i++)
                           {
                               for(int j =0;j<width;j++)
                               {       
                                   if(teams[i,j] == agentID[0] || teams[i,j] == agentID[1])
                                   {
                                       int[]  mawari;
                                       mawari = new int[8];
                                       mawari[0] = points[i-1,j-1]; mawari[1] = points[i-1,j]; mawari[2] = points[i-1,j+1];
                                       mawari[3] = points[i,j-1];                               mawari[4] = points[i,j+1];
                                       mawari[5] = points[i+1,j-1]; mawari[6] = points[i+1,j]; mawari[7] = points[i+1,j+1];
                                       Array.Sort(mawari);
                                   //mawari[8]; //これが一番大きい
                                   }
                               }
                           }
                       }
                 }

            周りが自分のタイルか否か？
            ↓
            周りで最も点数が高いものはどれか？
             }*/

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