using System.Drawing;

namespace FirstGame
{
    public class Bullet : Ship
    {
        //Images at screen
        Image defaultBullet = Image.FromFile("Content\\pictures\\bullet1.png");
        Image upgradeBullet = Image.FromFile("Content\\pictures\\bullet2.png");

        //Overloading (because we have 2 types of bullets)
        public override int Power
        {
            get { return power; }
            set { if (lvl > 3) power = 50; else power = value; }
        }

        //Constructor
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
        }

        //Drawing at game screen
        public override void Drawing()
        {
            if (lvl <= 3) GameFunctional.buffer.Graphics.DrawImage(defaultBullet, pos);
            else GameFunctional.buffer.Graphics.DrawImage(upgradeBullet, pos);
        }

        //Calculating new position of object
        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
        }
    }
}
