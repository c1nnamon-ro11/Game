using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;


namespace FirstGame
{
    public class ScoreMenu : Form
    {

        Label lblName;
        Label lblScore;
        Label lblUserScore;
        TextBox nameBox;
        Button btn_OK;
        public int Score { get; set; }
        public string PlayerName { get; set; }

        //Constructor
        public ScoreMenu(int width,int heigth,int _score)
        {
            this.Score = _score;
            lblName = new Label();
            lblScore = new Label();
            lblUserScore = new Label();
            nameBox = new TextBox();
            btn_OK = new Button();

            this.Controls.Add(lblName);
            this.Controls.Add(lblScore);
            this.Controls.Add(lblUserScore);
            this.Controls.Add(nameBox);
            this.Controls.Add(btn_OK);

            lblName.Left = 30;
            lblName.Top = 10;
            lblName.Text = "Name";

            lblScore.Left = width-100;
            lblScore.Top = 10;
            lblScore.Text = "Score";

            nameBox.Left = 10;
            nameBox.Top = heigth/2 - 60;

            lblUserScore.Left = width - 100;
            lblUserScore.Top = heigth / 2 - 60;
            lblUserScore.Text = Score.ToString();

            btn_OK.Size = new Size(100,50);
            btn_OK.Left = (width-btn_OK.Width)/2;
            btn_OK.Top = heigth - 120;
            btn_OK.Text = "OK";

            btn_OK.Click += Btn_OK_Click;
        }

        private void ScoreBoard()
        {
            int i = 1;
            string fileLine;
            List<string> users= new List<string>();
            StreamReader sr = new StreamReader("Content\\Scoreboard\\ScoreBoard.txt");
            while(!sr.EndOfStream)
            {
                fileLine = sr.ReadLine();
                users.Add(fileLine.Substring(fileLine.IndexOf(' ')+1));
            }
            sr.Close();

            Dictionary<string,int> scoreList = ScoreSort(users);
            StreamWriter sw = new StreamWriter("Content\\Scoreboard\\ScoreBoard.txt", false);
            foreach(var user in scoreList)
            {
                sw.WriteLine($"{i}) {user.Key} {user.Value}");
                i++;
            }
            sw.Close();
        }

        private Dictionary<string,int> ScoreSort(List<string> users)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add(PlayerName, Score);

            foreach (var user in users)
            {
                int space = user.IndexOf(' ');
                string name = user.Substring(0, space);
                int value = int.Parse(user.Substring(space));

                if (result.ContainsKey(name))
                {
                    if (result[name] < value) result[name] = value;
                }
                else { result[name] = value; }
            }

            result = result.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return result;
        }

        //Exit
        private void Btn_OK_Click(object sender, EventArgs e)
        {
            PlayerName = nameBox.Text;
            if (PlayerName.Length == 0) PlayerName = "UnknownPlayer";
            PlayerName = PlayerName.Replace(' ', '_');
            ScoreBoard();
            this.Close();
        }
    }
}
