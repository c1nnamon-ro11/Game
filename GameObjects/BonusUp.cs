using System.Drawing;

namespace FirstGame
{
    class BonusUp : Ship
    {
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\bonus.png");
        //Constructor
        public BonusUp(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
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
            if (pos.Y < Size) dir.Y = -dir.Y;
            if (pos.Y + Size > GameFunctional.Height) dir.Y = -dir.Y;
        }
    }
}
