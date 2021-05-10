using System;
using System.Drawing;

namespace FirstGame
{
    static class Background
    {
        //Background
        static public void BackGround()
        {           
            GameFunctional.buffer.Graphics.DrawImage
                (Image.FromFile(GameFunctional.texturePackPath+"planet.png"), new Rectangle(1000, 100, 100, 100));

            //"Star`s effect"
            Random rnd = new Random();
            int Num = rnd.Next(10, 100);
            int[] coorX = new int[Num];
            int[] coorY = new int[Num];
            for (int i = 0; i < Num; i++)
            {
                coorX[i] = rnd.Next(0, GameFunctional.Width);
                coorY[i] = rnd.Next(0, GameFunctional.Height);
                GameFunctional.buffer.Graphics.FillEllipse(Brushes.Bisque, new Rectangle(coorX[i], coorY[i], 2, 2));
            }           
        }

        //Display game info at top of the gamescreen
        static public void DisplayOutputGameInformation(BufferedGraphics buffer, Ship ship, int counter)
        {
            buffer.Graphics.DrawString("Energy:" + ship.Energy, SystemFonts.DefaultFont,
            Brushes.White, 0, 0);
            buffer.Graphics.DrawString("Score:" + ship.Score, SystemFonts.DefaultFont,
            Brushes.White, 100, 0);
            buffer.Graphics.DrawString("Number of shots:" + counter, SystemFonts.DefaultFont,
            Brushes.White, 200, 0);
            buffer.Render();
        }

        //Display information after game end
        static public void DisplayFinishGameStats(BufferedGraphics buffer)
        {
            buffer.Graphics.DrawString(
                "Game over", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline),
                Brushes.White, GameFunctional.Width / 2 - 250, GameFunctional.Height / 2 - 300);
            buffer.Graphics.DrawString(
                "Your score:" + Ship.ship.Score, new Font(FontFamily.GenericSansSerif, 30),
                Brushes.Bisque, GameFunctional.Width / 2 - 180, GameFunctional.Height / 2 - 200);
            buffer.Render();
        }        
    }
}
