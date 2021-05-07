using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class BonusUp : BaseObject
    {
        public static List<BonusUp> bonusUp = new List<BonusUp>();
        private static Random rnd = new Random();

        //Image at screen
        static Image img = Image.FromFile("Content\\pictures\\bonus.png");
        const int DEFAULT_POWER = 10;
        readonly int DEFAULT_WIDTH = img.Width;
        readonly int DEFAULT_HEIGHT = img.Height;

        //Constructor
        public BonusUp(Point pos, Point dir) : base(pos, dir)
        {
            power = DEFAULT_POWER;
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public BonusUp(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = DEFAULT_POWER;
        }

        public BonusUp(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public BonusUp(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
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
            if (pos.Y < HeightSize) dir.Y = -dir.Y;
            if (pos.Y + WidthSize > GameFunctional.Height) dir.Y = -dir.Y;
        }

        private void DestroyingObject(BonusUp bonus)
        {
            Ship.ship.ScoreUp(0);
            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(0);
            }
        }

        public static void Interaction()
        {
            foreach (var bonus in bonusUp)
            {
                bonus.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and ship
                    if (Ship.ship.Collision(bonus))
                    {
                        MusicEffects.BonusSound();
                        bonus.Power = 0;
                        if (Ship.ship.Lvl <= 5)
                        {
                            Ship.ship.LvlUp(1);
                        }
                        else Ship.ship.EnergyLow(-50); //increase HP if the level is greater than the specified
                        bonus.DestroyingObject(bonus);
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectsFromCollection()
        {
            bonusUp.RemoveAll(item => item.Power <= 0);
        }

        //Loading Bonus
        static public void LoadObject()
        {
            int speedX = rnd.Next(2, 3);
            int speedY = rnd.Next(2, 3);
            int sizeX = 45;
            int sizeY = 25;
            bonusUp.Add(new BonusUp(
                new Point(
                    GameFunctional.Width + rnd.Next(0, 400), GameFunctional.Height / 2 + rnd.Next(-400, 400)),
                new Point(-speedX, speedY)));
        }
    }
}
