using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class RocketV2 : BaseObject
    {
        //Class spawn variables
        public static List<RocketV2> rocketsV2 = new List<RocketV2>();
        private static Random rnd = new Random();

        //Image at screen
        static Image img = Image.FromFile(GameFunctional.texturePackPath + "rocketV2.png");

        //Default object characteristics
        const int DEFAULT_POWER = 10;
        const int DEFAULT_DAMAGE = 10;
        readonly int DEFAULT_WIDTH = img.Width;
        readonly int DEFAULT_HEIGHT = img.Height;

        //Constructors
        public RocketV2(Point pos, Point dir) : base(pos, dir)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public RocketV2(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
        }

        public RocketV2(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public RocketV2(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
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
        }

        //Procedure before removing game object from gamescreen
        public void DestroyingObject(RocketV2 rocketV2)
        {
            VisualEffect.LoadObjects(
                                rocketV2.PosX + rocketV2.WidthSize / 2, rocketV2.PosY + rocketV2.HeightSize / 2, 2); //Spawn in place of the object"visual effects"
            Ship.ship.ScoreUp(DEFAULT_POWER);
            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(DEFAULT_POWER);
            }
        }

        //Functional logic of Rocket
        public static void Interaction()
        {
            foreach (var rocketV2 in rocketsV2)
            {
                rocketV2.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    foreach (var bullet in Bullet.bullets)
                    {
                        if (bullet.Collision(rocketV2))
                        {
                            MusicEffects.HitSound();
                            rocketV2.PowerLow(bullet.Power);    //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            if (rocketV2.Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                rocketV2.DestroyingObject(rocketV2);
                            }
                        }
                    }
                    //Collision of object and ship 
                    if (Ship.ship.Collision(rocketV2))
                    {
                        MusicEffects.HitSound();
                        rocketV2.Power = 0;
                        Ship.ship.EnergyLow(rocketV2.Damage);
                        rocketV2.DestroyingObject(rocketV2);
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectsFromCollection()
        {
            rocketsV2.RemoveAll(
                item => (item.Power <= 0) || item.PosX + item.WidthSize < 0 || item.PosY > GameFunctional.Height || item.PosY < 0);
        }

        //Loading Rocket
        static public void LoadObjects(int numberOfRockets)
        {
            for (int i = 0; i < numberOfRockets; i++)
            {
                int r = rnd.Next(10, 15);
                rocketsV2.Add(new RocketV2(
                    new Point(GameFunctional.Width + rnd.Next(1, 100) * 10, rnd.Next(0, GameFunctional.Height)),
                    new Point(-r, r)));
            }
        }
    }
}
