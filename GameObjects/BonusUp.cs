using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class BonusUp : Ship
    {
        public static List<BonusUp> bonusUp = new List<BonusUp>();
        private static Random rnd = new Random();

        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\bonus.png");
        //Constructor
        public BonusUp(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = 10;
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

        private void DestroyingObject(BonusUp bonus, Ship ship)
        {
            ship.ScoreUp(30);
            if (!GameFunctional.isBossFight)
            {
                ship.BossTimeUp(30);
            }
        }

        public static void Interaction(Ship ship)
        {
            foreach (var bonus in bonusUp)
            {
                bonus.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and ship
                    if (ship.Collision(bonus))
                    {
                        MusicEffects.BonusSound();
                        bonus.Power = 0;
                        if (ship.Lvl <= 6)
                        {
                            ship.Lvl++;
                        }
                        else ship.EnergyLow(-50); //increase HP if the level is greater than the specified
                        bonus.DestroyingObject(bonus, ship);
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
            //int size = rnd.Next(50, 150);
            int sizeX = 45;
            int sizeY = 25;
            bonusUp.Add(new BonusUp(
                new Point(
                    GameFunctional.Width + rnd.Next(0, 400), GameFunctional.Height / 2 + rnd.Next(-400, 400)),
                new Point(-speedX, speedY), new Size(sizeX, sizeY)));
        }
    }
}
