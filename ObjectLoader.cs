using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstGame
{
    class ObjectLoader
    {
        //Randomize coordinate spawn
        static Random rnd = new Random();

        //Loading Asteroids
        static public void LoadAsteroids(int initialNumberOfAsteroids, List<Asteroid> asteroids)
        {
            
            for (int i = 0; i < initialNumberOfAsteroids; i++)
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

        //Loading Stars
        static public void LoadStars(int initialNumberOfStars, List<Star> stars)
        {
            for (int i = 0; i < initialNumberOfStars; i++)
            {
                int speedX = rnd.Next(5, 7);
                int speedY = rnd.Next(7, 10);
                int size = 25;
                stars.Add(new Star(
                    new Point(
                        GameFunctional.Width + GameFunctional.rnd.Next(1, 100) * 10, rnd.Next(0, GameFunctional.Height)),
                    new Point(-speedX, speedY), new Size(size, size)));
            }
            initialNumberOfStars++;
        }

        //Loading Rocket
        static public void LoadRockets(int initialNumberOfRockets, List<Rocket> rockets)
        {
            for (int i = 0; i < initialNumberOfRockets; i++)
            {
                int r = rnd.Next(10, 15);
                //int size = 30;
                rockets.Add(new Rocket(
                    new Point(
                        GameFunctional.Width + GameFunctional.rnd.Next(1, 100) * 10, rnd.Next(0, GameFunctional.Height)),
                    new Point(-r, r), new Size(70, 20)));
            }
            initialNumberOfRockets++;
        }

        //Loading Bonus
        static public void LoadBonus(ref int bonusTime, List<BonusUp> bonus)
        {
            int speedX = rnd.Next(2, 3);
            int speedY = rnd.Next(2, 3);
            //int size = rnd.Next(50, 150);
            int sizeX = 45;
            int sizeY = 25;
            bonus.Add(new BonusUp(
                new Point(
                    GameFunctional.Width + rnd.Next(0, 400), GameFunctional.Height / 2 + rnd.Next(-400, 400)),
                new Point(-speedX, speedY), new Size(sizeX, sizeY)));
            bonusTime++;
        }

        //Loading Boss
        static public void LoadBoss(ref bool isBossFight, List<Boss> boss, Ship ship)
        {
            isBossFight = true;
            boss.Add(new Boss(
                    new Point(
                        GameFunctional.Width + rnd.Next(10, 100), GameFunctional.Height / 2),
                    new Point(-3, 3), new Size(350, 350)));
            ship.BossTime = 0;
        }

        //"Visual effects"
        static public void VisualEffect(int n, int PosX, int PosY, List<VisualEffect> visualEffects)
        {
            for (int ich = -n; ich <= n; ich++)
            {
                for (int jch = -n; jch <= n; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    visualEffects.Add(new VisualEffect(
                        new Point(PosX, PosY),
                        new Point(3 * ich + rnd.Next(-7, 7), 3 * jch + rnd.Next(-7, 7)), new Size(5, 5)));
                }
            }
        }
    }
}
