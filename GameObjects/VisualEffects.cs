using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    public class VisualEffect : BaseObject
    {
        //Class spawn variables
        public static List<VisualEffect> visualEffects = new List<VisualEffect>();
        private static Random rnd = new Random();
        private bool isColorEffect;

        //Default object characteristics
        readonly int DEFAULT_WIDTH = 5;
        readonly int DEFAULT_HEIGHT = 5;

        //Constructors
        public VisualEffect(Point pos, Point dir) : base(pos, dir)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public VisualEffect(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public VisualEffect(Point pos, Point dir, bool isDifferentColor) : base(pos, dir)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            isColorEffect = isDifferentColor;
        }

        public VisualEffect(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public VisualEffect(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
        {
        }

        //Drawing at game screen
        public override void Drawing()
        {
            if (isColorEffect)
            {
                GameFunctional.buffer.Graphics.FillEllipse(Brushes.Gold, pos.X, pos.Y, WidthSize, HeightSize);
            }
            else GameFunctional.buffer.Graphics.FillEllipse(Brushes.Aquamarine, pos.X, pos.Y, WidthSize, HeightSize);
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

        //Procedure before removing game object from gamescreen
        public void DestroyingObject()
        {
        }

        //Functional logic of Rocket
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
        static public void LoadObjects(int posX, int posY, int numberOfObject, bool isDifferentColor = false)
        {
            for (int ich = -numberOfObject; ich <= numberOfObject; ich++)
            {
                for (int jch = -numberOfObject; jch <= numberOfObject; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    visualEffects.Add(new VisualEffect(
                        new Point(posX, posY),
                        new Point(3 * ich + rnd.Next(-7, 7), 3 * jch + rnd.Next(-7, 7)),
                        isDifferentColor));
                }
            }
        }
    }
}
