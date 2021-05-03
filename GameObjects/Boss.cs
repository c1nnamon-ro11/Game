using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class Boss : BaseObject
    {
        public static Boss boss;/* = new Boss(
                    new Point(GameFunctional.Width + rnd.Next(10, 100), GameFunctional.Height / 2),
                    new Point(-3, 3),
                    new Size(350, 350));*/

        private static Random rnd = new Random();
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\boss.png");
        //Constructor
        public Boss(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = 200;
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
            if (pos.X > GameFunctional.Width - Size) dir.X = -rnd.Next(2, 5);
            if (pos.Y < 0) dir.Y = rnd.Next(2, 5);
            if (pos.Y > GameFunctional.Height - Size) dir.Y = -rnd.Next(2, 5);
        }

        private void DestroyingObject(Boss boss, Ship ship)
        {
            GameFunctional.isBossFight = false;
            VisualEffect.LoadObjects(
                            boss.PosX + boss.Size / 2, boss.PosY + boss.Size / 2, 5);  //Spawn in place of the object of "visual effects
            ship.ScoreUp(5000);
            GameFunctional.bonusTime += 25;
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

        public static void Interaction(List<Bullet> bullets, Ship ship, ref int index)
        {
            //Boss
            if (GameFunctional.startGame)
            {
                if (GameFunctional.isBossFight)
                {
                    boss.Update();
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        //Collision of object and bullet
                        if (bullets[j].Collision(boss))
                        {
                            MusicEffects.HitSound();
                            boss.PowerLow(bullets[j].Power); //Object "power" reduction
                            bullets.RemoveAt(j);
                            j--; index--;
                            //Spawn in boss place charges
                            AsteroidCharge.LoadAsteroidCharges(boss.PosX, boss.PosY, boss.Size, 1, true);
                            //Boss destoring procedure
                            if (boss.Power <= 0)
                            {
                                MusicEffects.BonusSound();
                                ship.LvlUp(1);
                                boss.DestroyingObject(boss, ship);
                            }
                        }
                    }
                    //End the game if ship collides with boss
                    if (ship.Collision(boss))
                    {
                        ship.Energy = 0;
                    }
                }
            }
            RemoveObjectFromCollection();
        }

        //Loading Boss
        static public void LoadObjects(Ship ship)
        {
            boss = new Boss(new Point(GameFunctional.Width + rnd.Next(10, 100), GameFunctional.Height / 2),
                    new Point(-3, 3),
                    new Size(350, 350));
            GameFunctional.isBossFight = true;
            ship.BossTime = 0;
        }
    }
}
