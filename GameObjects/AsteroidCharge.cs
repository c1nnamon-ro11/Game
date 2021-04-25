using System.Drawing;

namespace FirstGame
{
    class AsteroidCharge : Asteroid
    {
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\charge.png");
        //Constructor with "power"
        public AsteroidCharge(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 5;
            Damage = 5;
        }

        //Drawing at game screen
        public override void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (img, pos);
        }

        //Calculating new position of object
        override public void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
            if (dir.X == 0 && dir.Y == 0)
            {
                dir.X = GameFunctional.rnd.Next(-1, 13);
                dir.Y = GameFunctional.rnd.Next(-4, 10);
            }
        }
    }
}
