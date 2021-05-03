using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class Asteroid : BaseObject
    {
        public static List<Asteroid> asteroids = new List<Asteroid>();
        private static Random rnd = new Random();

        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\asteroid.png");

        //Constructor with "power"
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 50;
            Damage = 15;
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

        private void DestroyingObject(Asteroid asteroid, Ship ship)
        {
            AsteroidCharge.LoadAsteroidCharges(asteroid.PosX, asteroid.PosY, asteroid.Size, 1);
            VisualEffect.LoadObjects(
                                asteroid.PosX + asteroid.Size / 2, asteroid.PosY + asteroid.Size / 2, 2); //Spawn in place of the object"visual effects"
            ship.ScoreUp(asteroid.Size);

            if (!GameFunctional.isBossFight)
            {
                ship.BossTimeUp(asteroid.Size);
            }
        }

        public static void Interaction(List<Bullet> bullets, Ship ship, ref int index)
        {
            foreach (var asteroid in asteroids)
            {
                asteroid.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].Collision(asteroid))
                        {
                            MusicEffects.HitSound();
                            asteroid.PowerLow(bullets[j].Power);    //Object "power" reduction
                            if (asteroid.Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                asteroid.DestroyingObject(asteroid, ship);
                            }
                            bullets.RemoveAt(j);
                            j--;
                            index--;
                        }
                    }
                    //Collision of object and ship 
                    if (ship.Collision(asteroid))
                    {
                        MusicEffects.HitSound();
                        asteroid.Power = 0;
                        ship.EnergyLow(asteroid.Damage);
                        ship.LvlUp(-1);
                        asteroid.DestroyingObject(asteroid, ship);
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectsFromCollection()
        {
            //int numberOfObjectsBeforeRemoving = asteroids.Count;

            asteroids.RemoveAll(item => item.Power <= 0);

            /*if (!GameFunctional.isBossFight)
            {
                LoadObjects(numberOfObjectsBeforeRemoving - asteroids.Count);
            }*/
        }

        //Loading Asteroids
        static public void LoadObjects(int numberOfAsteroids)
        {
            for (int i = 0; i < numberOfAsteroids; i++)
            {
                int speedX = rnd.Next(2, 7);
                int speedY = rnd.Next(2, 7);
                //int size = rnd.Next(50, 150);
                int size = 70;
                asteroids.Add(new Asteroid(
                    new Point(
                        GameFunctional.Width + rnd.Next(100, 400), GameFunctional.Height / 2 - rnd.Next(-200, 200)),
                    new Point(-speedX, speedY), new Size(size, size)));
            }
        }
    }
}
