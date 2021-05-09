using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class Boss : BaseObject
    {
        //Class spawn variables
        public static Boss boss;
        private static Random rnd = new Random();

        //Image at screen
        static Image img = Image.FromFile(GameFunctional.texturePackPath + "boss.png");

        //Default object characteristics
        public const int DEFAULT_POWER = 300;
        public const int DEFAULT_DAMAGE = int.MaxValue;
        static readonly int DEFAULT_WIDTH = img.Width;
        static readonly int DEFAULT_HEIGHT = img.Height;

        public bool ulta;
        private double multiplier = 2.0/3;

        //Constructors
        public Boss(Point pos, Point dir) : base(pos, dir)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public Boss(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;            
        }

        public Boss(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public Boss(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
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

            if (pos.X < GameFunctional.Width / 2) dir.X = rnd.Next(2, 5);
            if (pos.X > GameFunctional.Width - HeightSize) dir.X = -rnd.Next(2, 5);
            if (pos.Y < 0) dir.Y = rnd.Next(2, 5);
            if (pos.Y > GameFunctional.Height - WidthSize) dir.Y = -rnd.Next(2, 5);
        }

        //Procedure before removing game object from gamescreen
        private void DestroyingObject(Boss boss)
        {
            GameFunctional.isBossFight = false;
            VisualEffect.LoadObjects(
                            boss.PosX + boss.WidthSize / 2, boss.PosY + boss.HeightSize / 2, 5);  //Spawn in place of the object of "visual effects
            Ship.ship.ScoreUp(DEFAULT_POWER);
            GameFunctional.bonusTime += 1;
            GameFunctional.Load();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectFromCollection()
        {
            if(boss != null && boss.Power <= 0)
            {
                boss = null;
            }
        }

        //Functional logic of Boss
        public static void Interaction()
        {
            //Boss
            if (GameFunctional.startGame)
            {
                if (GameFunctional.isBossFight)
                {
                    boss.Update();
                    foreach (var bullet in Bullet.bullets)
                    {
                        //Collision of object and bullet
                        if (bullet.Collision(boss))
                        {
                            MusicEffects.HitSound();
                            boss.PowerLow(bullet.Power); //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            //Spawn in boss place charges
                            AsteroidCharge.LoadAsteroidCharges(boss.PosX, boss.PosY, boss.WidthSize, boss.HeightSize, 1, true);
                            if(boss.Power < DEFAULT_POWER * boss.multiplier)
                            {
                                boss.ulta = true;
                                boss.multiplier /= 2;
                            }
                            //Boss destoring procedure
                            if (boss.Power <= 0)
                            {
                                MusicEffects.BonusSound();
                                Ship.ship.LvlUp(1);
                                boss.DestroyingObject(boss);
                            }
                        }
                    }
                    //End the game if ship collides with boss
                    if (Ship.ship.Collision(boss))
                    {
                        Ship.ship.Energy = 0;
                    }
                }
            }
            RemoveObjectFromCollection();
        }

        //Loading Boss
        static public void LoadObjects()
        {
            boss = new Boss(
                new Point(GameFunctional.Width + rnd.Next(10, 100), GameFunctional.Height / 2),
                new Point(-3, 3));
            GameFunctional.isBossFight = true;
            Ship.ship.BossTime = 0;
        }
    }
}
