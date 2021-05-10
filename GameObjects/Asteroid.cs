using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class Asteroid : BaseObject
    {
        //Class spawn variables
        public static List<Asteroid> asteroids = new List<Asteroid>();
        private static Random rnd = new Random();
        private static Random r = new Random();

        //Image at screen
        Image asteroidImg = ChooseObjectSkin(); 

        //Default object characteristics
        const int DEFAULT_POWER = 100;
        const int DEFAULT_DAMAGE = 15;

        //Constructors
        public Asteroid(Point pos, Point dir) : base(pos, dir)
        {
            size = new Size(asteroidImg.Width, asteroidImg.Height);
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
        }

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            asteroidImg = ChooseObjectSkin();
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
        }

        public Asteroid(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            asteroidImg = ChooseObjectSkin();
            size = new Size(asteroidImg.Width, asteroidImg.Height);
        }

        public Asteroid(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
        {
            asteroidImg = ChooseObjectSkin();
        }

        //Choosing random skin for current object
        static private Image ChooseObjectSkin()
        {
            switch (r.Next(1, 4))
            {
                case 1:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroid\\asteroid1.png");                   
                case 2:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroid\\asteroid2.png");
                case 3:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroid\\asteroid3.png");
                case 4:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroid\\asteroid4.png");
                default:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroid\\asteroid1.png");
            }            
        } 

        //Drawing at game screen
        override public void Drawing()
        {            
            GameFunctional.buffer.Graphics.DrawImage(asteroidImg, pos);
        }

        //Calculating new position of object
        override public void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
            if (pos.X + WidthSize + 20 < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = rnd.Next(0, GameFunctional.Height);
            }
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y + HeightSize > GameFunctional.Height) dir.Y = -dir.Y;
        }

        //Procedure before removing game object from gamescreen
        public void DestroyingObject(Asteroid asteroid, bool isDestroyedByBullet = true)
        {
            if (isDestroyedByBullet)
            {
                AsteroidCharge.LoadAsteroidCharges(asteroid.PosX, asteroid.PosY, asteroid.HeightSize, asteroid.WidthSize, 1);
            }          
            VisualEffect.LoadObjects(
                                asteroid.PosX + asteroid.WidthSize / 2, asteroid.PosY + asteroid.HeightSize / 2, 2); //Spawn in place of the object"visual effects"
            Ship.ship.ScoreUp(DEFAULT_POWER);

            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(DEFAULT_POWER);
            }
        }

        //Functional logic of Asteroid
        public static void Interaction()
        {
            foreach (var asteroid in asteroids)
            {
                asteroid.Update();               
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    foreach (var bullet in Bullet.bullets)
                    {
                        if (bullet.Collision(asteroid))
                        {
                            MusicEffects.HitSound();
                            asteroid.PowerLow(bullet.Power);    //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            if (asteroid.Power <= 0)    //Procedure or destroying object (if power less then zero)
                            {
                                asteroid.DestroyingObject(asteroid);
                            }
                        }
                    }
                    //Collision of object and ship 
                    if (Ship.ship.Collision(asteroid))
                    {
                        MusicEffects.HitSound();
                        asteroid.Power = 0;
                        Ship.ship.EnergyLow(asteroid.Damage);
                        Ship.ship.LvlUp(-1);
                        asteroid.DestroyingObject(asteroid, false);                        
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectsFromCollection()
        {
            asteroids.RemoveAll(item => item.Power <= 0);
        }

        //Loading Asteroids
        static public void LoadObjects(int numberOfAsteroids)
        {
            for (int i = 0; i < numberOfAsteroids; i++)
            {
                int speedX = rnd.Next(2, 7);
                int speedY = rnd.Next(2, 7);
                asteroids.Add(new Asteroid(
                    new Point(GameFunctional.Width + rnd.Next(100, 400), GameFunctional.Height / 2 - rnd.Next(-150, 150)),
                    new Point(-speedX, speedY)));
            }
        }
    }
}
