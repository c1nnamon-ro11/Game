using System.Drawing;

namespace FirstGame
{
    class Rocket : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\rocket.png");

        //Constructor with "power"
        public Rocket(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 5;
            Damage = 10;
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
            if (pos.X + Size + 20 < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = GameFunctional.rnd.Next(0, GameFunctional.Height);
                dir.X = -GameFunctional.rnd.Next(10, 15);
            }
        }
    }
}
