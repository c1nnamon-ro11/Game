using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class AsteroidCharge : BaseObject
    {
        //Class spawn variables
        public static List<AsteroidCharge> asteroidCharges = new List<AsteroidCharge>();
        private static Random rnd = new Random();

        //Image at screen
        Image asteroidChargeImg = ChooseObjectSkin();

        //Default object characteristics
        const int DEFAULT_POWER = 100;
        const int DEFAULT_DAMAGE = 15;

        //Constructors
        public AsteroidCharge(Point pos, Point dir) : base(pos, dir)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
            size = new Size(asteroidChargeImg.Width, asteroidChargeImg.Height);
        }

        public AsteroidCharge(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
        }

        public AsteroidCharge(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(asteroidChargeImg.Width, asteroidChargeImg.Height);
        }

        public AsteroidCharge(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
        {
        }

        //Choosing random skin for current object
        static private Image ChooseObjectSkin()
        {
            switch (rnd.Next(1, 4))
            {
                case 1:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroidCharge\\asteroidCharge1.png");
                case 2:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroidCharge\\asteroidCharge2.png");
                case 3:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroidCharge\\asteroidCharge3.png");
                default:
                    return Image.FromFile(GameFunctional.texturePackPath + "asteroidCharge\\asteroidCharge1.png");
            }
        }

        //Drawing at game screen
        public override void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (asteroidChargeImg, pos);
        }

        //Calculating new position of object
        override public void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
            if (dir.X == 0 && dir.Y == 0)
            {
                dir.X = rnd.Next(-1, 13);
                dir.Y = rnd.Next(-4, 10);
            }
        }

        //Procedure before removing game object from gamescreen
        public void DestroyingObject(AsteroidCharge asteroidCharge)
        {
            VisualEffect.LoadObjects(
                                asteroidCharge.PosX + asteroidCharge.HeightSize / 2, asteroidCharge.PosY + asteroidCharge.WidthSize / 2, 2); //Spawn in place of the object"visual effects"
            Ship.ship.ScoreUp(DEFAULT_POWER);
            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(DEFAULT_POWER);
            }
        }

        //Functional logic of Asteroid
        static public void Interaction()
        {
            //Asteroids charges
            foreach (var asteroidCharge in asteroidCharges)
            {
                asteroidCharge.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    foreach (var bullet in Bullet.bullets)
                    {
                        if (bullet.Collision(asteroidCharge))
                        {
                            MusicEffects.HitSound();
                            asteroidCharge.PowerLow(bullet.Power);  //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            if (asteroidCharge.Power <= 0) //Procedure or destroyingobject (if power less then zero)
                            {
                                asteroidCharge.DestroyingObject(asteroidCharge);
                            }
                        }
                    }
                    //Collision of object and ship
                    if (Ship.ship.Collision(asteroidCharge))
                    {
                        MusicEffects.HitSound();
                        asteroidCharge.Power = 0;
                        Ship.ship.EnergyLow(asteroidCharge.Damage);
                        Ship.ship.ScoreUp(10);
                        asteroidCharge.DestroyingObject(asteroidCharge);
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        static private void RemoveObjectsFromCollection()
        {
            asteroidCharges.RemoveAll(item => item.Power <= 0);
        }

        //Loading Asteroid charge
        static public void LoadAsteroidCharges(int posX, int posY, int posSizeWidth, int posSizeHeight, int koefOfNumber, bool isBossCharge=false)
        {
            for (int ich = -koefOfNumber; ich <= koefOfNumber; ich++)
            {
                for (int jch = -koefOfNumber; jch <= koefOfNumber; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    if (ich == 0 || jch == 0) { continue; }
                    if (isBossCharge)
                    {
                        asteroidCharges.Add(new AsteroidCharge(
                            new Point(posX + posSizeWidth / 2 + ich * posSizeWidth / 4, posY + posSizeHeight / 2 + jch * posSizeHeight / 4),
                            new Point(6 * ich + rnd.Next(-7, 7), 3 * jch + rnd.Next(-7, 7)), 5, 5));
                    }
                    else
                    {
                        asteroidCharges.Add(new AsteroidCharge(
                            new Point(posX + posSizeWidth / 2, posY + posSizeHeight / 2),
                            new Point(3 * ich, 3 * jch)));
                    }
                }
            }
        }
    }
}
