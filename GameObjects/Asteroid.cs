using System.Drawing;

namespace FirstGame
{
    class Asteroid : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\asteroid.png");

        //Constructor with "power"
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 50;
            Damage = 15;
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
            if (pos.X + Size + 20 < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = GameFunctional.rnd.Next(0, GameFunctional.Height);
            }
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y + Size > GameFunctional.Height) dir.Y = -dir.Y;
        }
    }
}
