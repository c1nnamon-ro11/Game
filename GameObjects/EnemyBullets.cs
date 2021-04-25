using System.Drawing;

namespace FirstGame
{
    class EnemyBullets : Boss
    {
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\fireball2.png");
        //Constructor
        public EnemyBullets(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
            Damage = 5;
        }

        //Drawing at game screen
        override public void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (img, pos);
        }

        //Calculating new position of object
        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
        }
    }
}
