using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class Rocket : BaseObject
    {
        public static List<Rocket> rockets = new List<Rocket>();
        private static Random rnd = new Random();

        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\rocket.png");

        //Constructor with "power"
        public Rocket(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 5;
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
            if (pos.X + Size + 20 < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = rnd.Next(0, GameFunctional.Height);
                dir.X = -rnd.Next(10, 15);
            }
        }

        public void DestroyingObject(Rocket rocket)
        {
            VisualEffect.LoadObjects(
                                rocket.PosX + rocket.Size / 2, rocket.PosY + rocket.Size / 2, 2); //Spawn in place of the object"visual effects"
            Ship.ship.ScoreUp(rocket.Size);
            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(rocket.Size);
            }
        }

        public static void Interaction()
        {
            foreach (var rocket in rockets)
            {
                rocket.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    foreach (var bullet in Bullet.bullets)
                    {
                        if (bullet.Collision(rocket))
                        {
                            MusicEffects.HitSound();
                            rocket.PowerLow(bullet.Power);    //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            if (rocket.Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                rocket.DestroyingObject(rocket);
                            }
                        }
                    }
                    //Collision of object and ship 
                    if (Ship.ship.Collision(rocket))
                    {
                        MusicEffects.HitSound();
                        rocket.Power = 0;
                        Ship.ship.EnergyLow(rocket.Damage);
                        rocket.DestroyingObject(rocket);
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectsFromCollection()
        {
            rockets.RemoveAll(item => item.Power <= 0);
        }

        //Loading Rocket
        static public void LoadObjects(int numberOfRockets)
        {
            for (int i = 0; i < numberOfRockets; i++)
            {
                int r = rnd.Next(10, 15);
                //int size = 30;
                rockets.Add(new Rocket(
                    new Point(
                        GameFunctional.Width + rnd.Next(1, 100) * 10, rnd.Next(0, GameFunctional.Height)),
                    new Point(-r, r), new Size(70, 20)));
            }
        }
    }
}
