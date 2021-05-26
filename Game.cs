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
            //Full screen
            GameScreen.WindowState = FormWindowState.Maximized;           
            GameFunctional.Initialization(GameScreen);
            //Screen`s name
            GameScreen.Text = "Asteroid Belt";
            GameScreen.Show();
            GameFunctional.Drawing();
            Application.Run(GameScreen);
        }
    }
}
