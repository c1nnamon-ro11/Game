using System.Drawing;

namespace FirstGame
{
    class Boss : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\boss.png");
        //Constructor
        public Boss(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = 2000;
        }

        //Drawing at game screen
        override public void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (img, pos);
        }

        //Calculating new position of object
        override public void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;

            if (pos.X < GameFunctional.Width / 2) dir.X = GameFunctional.rnd.Next(2, 5);
            if (pos.X > GameFunctional.Width - Size) dir.X = -GameFunctional.rnd.Next(2, 5);
            if (pos.Y < 0) dir.Y = GameFunctional.rnd.Next(2, 5);
            if (pos.Y > GameFunctional.Height - Size) dir.Y = -GameFunctional.rnd.Next(2, 5);
        }
    }
}
