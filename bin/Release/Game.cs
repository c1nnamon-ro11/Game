using System.Windows.Forms;

namespace FirstGame
{
    static class Game
    {
        static void Main(string[] args)
        {
            //Initialization and game size
            //WindowsBase.dll and PresentationCore.dll
            Form GameScreen = new Form();
            GameScreen.WindowState = FormWindowState.Maximized;

            //GameScreen.Width = 1920;
            //GameScreen.Height = 1440;           
            GameFunctional.Initialization(GameScreen);
            GameScreen.Text = "Asteroid Belt";
            GameScreen.Show();
            GameFunctional.Drawing();
            Application.Run(GameScreen);
        }
    }
}
