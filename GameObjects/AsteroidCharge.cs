using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class AsteroidCharge : Asteroid
    {
        public static List<AsteroidCharge> asteroidCharges = new List<AsteroidCharge>();
        private static Random rnd = new Random();

        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\charge.png");
        //Constructor with "power"
        public AsteroidCharge(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 5;
            Damage = 5;
        }

        //Drawing at game screen
        public override void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (img, pos);
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

        private void DestroyingObject(AsteroidCharge asteroidCharge, Ship ship)
        {
            VisualEffect.LoadObjects(
                                asteroidCharge.PosX + asteroidCharge.Size / 2, asteroidCharge.PosY + asteroidCharge.Size / 2, 2); //Spawn in place of the object"visual effects"
            ship.ScoreUp(asteroidCharge.Size);
            if (!GameFunctional.isBossFight)
            {
                ship.BossTimeUp(asteroidCharge.Size);
            }
        }

        static public void Interaction(List<Bullet> bullets, Ship ship, ref int index)
        {
            //Asteroids charges
            foreach (var asteroidCharge in asteroidCharges)
            {
                asteroidCharge.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and bullet  
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].Collision(asteroidCharge))
                        {
                            MusicEffects.HitSound();
                            asteroidCharge.PowerLow(bullets[j].Power);  //Object "power" reduction
                            if (asteroidCharge.Power <= 0) //Procedure or destroyingobject (if power less then zero)
                            {
                                asteroidCharge.DestroyingObject(asteroidCharge, ship);
                            }
                            bullets.RemoveAt(j);
                            j--; index--;
                        }
                    }
                    //Collision of object and ship
                    if (ship.Collision(asteroidCharge))
                    {
                        MusicEffects.HitSound();
                        asteroidCharge.Power = 0;
                        ship.EnergyLow(asteroidCharge.Damage);
                        ship.ScoreUp(10);
                        asteroidCharge.DestroyingObject(asteroidCharge, ship);
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
        static public void LoadAsteroidCharges(int posX, int posY, int sizeH, int n, bool isBossCharge=false)
        {
            for (int ich = -n; ich <= n; ich++)
            {
                for (int jch = -n; jch <= n; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    if (ich == 0 || jch == 0) { continue; }
                    if (isBossCharge)
                    {
                        asteroidCharges.Add(new AsteroidCharge(
                            new Point(posX + sizeH / 2 + ich * sizeH / 4, posY + sizeH / 2 + jch * sizeH / 4),
                            new Point(6 * ich + rnd.Next(-7, 7), 3 * jch + rnd.Next(-7, 7)), new Size(30, 30)));
                    }
                    else
                    {
                        asteroidCharges.Add(new AsteroidCharge(
                            new Point(posX + sizeH / 2, posY + sizeH / 2),
                            new Point(3 * ich, 3 * jch), new Size(sizeH / 4, sizeH / 4)));
                    }
                }
            }
        }
    }
}
