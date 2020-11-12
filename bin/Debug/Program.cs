using System;
using System.Windows.Forms;
using System.Drawing;

namespace FirstGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialization and game size
            Form GameScreen = new Form();
            GameScreen.Width = 1920;
            GameScreen.Height = 1080;           
            GameFunctional.Initialization(GameScreen);
            GameScreen.Show();
            GameFunctional.Drawing();
            Application.Run(GameScreen);
        }
    }
}
