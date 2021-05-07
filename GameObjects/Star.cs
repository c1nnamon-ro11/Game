using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    public class Star : BaseObject
    {
        public static List<Star> stars = new List<Star>();
        private static Random rnd = new Random();

        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\star.png");

        //Constructor with "power"
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
            Damage = 10;
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
            if (pos.X + Size + 20 < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = rnd.Next(0, GameFunctional.Height);
            }
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y + Size > GameFunctional.Height) dir.Y = -dir.Y;
        }

        public void DestroyingObject(Star star)
        {
            VisualEffect.LoadObjects(
                                star.PosX + star.Size / 2, star.PosY + star.Size / 2, 2); //Spawn in place of the object"visual effects"
            Ship.ship.ScoreUp(star.Size);
            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(star.Size);
            }
        }

        public static void Interaction()
        {
            foreach (var star in stars)
            {
                star.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    foreach (var bullet in Bullet.bullets)
                    {
                        if (bullet.Collision(star))
                        {
                            MusicEffects.HitSound();
                            star.PowerLow(bullet.Power);    //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            if (star.Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                star.DestroyingObject(star);
                            }
                        }
                    }
                    //Collision of object and ship 
                    if (Ship.ship.Collision(star))
                    {
                        MusicEffects.HitSound();
                        star.Power = 0;
                        Ship.ship.EnergyLow(star.Damage);
                        star.DestroyingObject(star);
                    }
                }
            }
            RemoveObjectFromCollection();
        }       

        //Removing objects from gamescreen
        public static void RemoveObjectFromCollection()
        {
            stars.RemoveAll(item => item.Power <=0);
        }

        //Loading Stars
        static public void LoadObjects(int numberOfStars)
        {
            for (int i = 0; i < numberOfStars; i++)
            {
                int speedX = rnd.Next(5, 7);
                int speedY = rnd.Next(7, 10);
                int size = 25;
                stars.Add(new Star(
                    new Point(
                        GameFunctional.Width + rnd.Next(1, 100) * 10, rnd.Next(0, GameFunctional.Height)),
                    new Point(-speedX, speedY), new Size(size, size)));
            }
        }
    }
}
