using System.Drawing;

namespace FirstGame
{
    class VisualEffect : BaseObject
    {
        //Constructor
        public VisualEffect(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        //Drawing at game screen
        public override void Drawing()
        {
            GameFunctional.buffer.Graphics.FillEllipse(
                Brushes.Aquamarine, pos.X, pos.Y, size.Width, size.Height);
        }

        //Calculating new position of object
        override public void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
            if (dir.X == 0 && dir.Y == 0)
            {
                dir.X = GameFunctional.rnd.Next(-5, 5);
                dir.Y = GameFunctional.rnd.Next(-5, 5);

            }
        }
    }
}
