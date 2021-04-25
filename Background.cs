using System;
using System.Drawing;

namespace FirstGame
{
    class Background
    {
        //Background
        static public void BackGround()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (Image.FromFile("Content\\pictures\\planet.png"), new Rectangle(1000, 100, 100, 100));

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
    }
}
