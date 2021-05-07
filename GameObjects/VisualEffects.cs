using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    public class VisualEffect : BaseObject
    {
        public static List<VisualEffect> visualEffects = new List<VisualEffect>();
        private static Random rnd = new Random();
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
                dir.X = rnd.Next(-5, 5);
                dir.Y = rnd.Next(-5, 5);

            }
        }

        public void DestroyingObject()
        {
        }

        public static void Interaction()
        {
            foreach (var visualEffect in visualEffects)
            {
                visualEffect.Update();
            }
            RemoveObjectFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectFromCollection()
        {
            visualEffects.RemoveAll(
                item => (item.PosX > GameFunctional.Width || item.PosX < 0 || item.PosY > GameFunctional.Height || item.PosY < 0));
        }

        //Loading Rocket
        static public void LoadObjects(int posX, int posY, int numberOfObject)
        {
            for (int ich = -numberOfObject; ich <= numberOfObject; ich++)
            {
                for (int jch = -numberOfObject; jch <= numberOfObject; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    visualEffects.Add(new VisualEffect(
                        new Point(posX, posY),
                        new Point(3 * ich + rnd.Next(-7, 7), 3 * jch + rnd.Next(-7, 7)), new Size(5, 5)));
                }
            }
        }
    }
}
