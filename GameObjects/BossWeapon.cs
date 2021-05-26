using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class BossWeapon : BaseObject
    {
        //Class spawn variables
        public static List<BossWeapon> bossWeapons = new List<BossWeapon>();
        private static Random rnd = new Random();

        //Image at screen
        static Image img = Image.FromFile(GameFunctional.texturePackPath + "bossWeapon.png");

        //Default object characteristics
        const int DEFAULT_POWER = 10;
        const int DEFAULT_DAMAGE = 10;
        readonly int DEFAULT_WIDTH = img.Width;
        readonly int DEFAULT_HEIGHT = img.Height;

        //Constructors
        public BossWeapon(Point pos, Point dir) : base(pos, dir)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public BossWeapon(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
        }

        public BossWeapon(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public BossWeapon(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
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
        public void DestroyingObject(BossWeapon bossWeapon)
        {
            VisualEffect.LoadObjects(
                                bossWeapon.PosX + bossWeapon.WidthSize / 2, bossWeapon.PosY + bossWeapon.HeightSize / 2, 2); //Spawn in place of the object"visual effects"
            Ship.ship.ScoreUp(DEFAULT_POWER);
            if (!GameFunctional.isBossFight)
            {
                Ship.ship.BossTimeUp(DEFAULT_POWER);
            }
        }

        //Functional logic of Rocket
        public static void Interaction()
        {
            foreach (var bossWeapon in bossWeapons)
            {
                bossWeapon.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    foreach (var bullet in Bullet.bullets)
                    {
                        if (bullet.Collision(bossWeapon))
                        {
                            MusicEffects.HitSound();
                            bossWeapon.PowerLow(bullet.Power);    //Object "power" reduction
                            Bullet.DestroyingObject(bullet);
                            if (bossWeapon.Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                bossWeapon.DestroyingObject(bossWeapon);
                            }
                        }
                    }
                    //Collision of object and ship 
                    if (Ship.ship.Collision(bossWeapon))
                    {
                        MusicEffects.HitSound();
                        bossWeapon.Power = 0;
                        Ship.ship.EnergyLow(bossWeapon.Damage);
                        bossWeapon.DestroyingObject(bossWeapon);
                    }
                }
            }
            RemoveObjectsFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectsFromCollection()
        {
            bossWeapons.RemoveAll(
                item => (item.Power <= 0) || item.PosX + item.WidthSize < 0 || item.PosY > GameFunctional.Height || item.PosY < 0);
        }

        //Loading Rocket
        static public void LoadObjects(int posX, int posY, int posSizeWidth, int posSizeHeigth)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                bossWeapons.Add(new BossWeapon(
                    new Point(posX + posSizeWidth/4, posY + posSizeHeigth/2 + i*img.Height*2 - img.Height/2),
                    new Point(-5, 0)));
            }
        }
    }
}
