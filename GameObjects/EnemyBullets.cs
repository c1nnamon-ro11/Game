using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class EnemyBullets : BaseObject
    {
        public static List<EnemyBullets> enemyBullets = new List<EnemyBullets>();
        private static Random rnd = new Random();
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\fireball2.png");
        //Constructor
        public EnemyBullets(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
            Damage = 5;
        }

        //Drawing at game screen
        override public void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (img, pos);
        }

        //Calculating new position of object
        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
        }

        private void DestroyingObject(EnemyBullets enemyBullet)
        {
            VisualEffect.LoadObjects(
                            enemyBullet.PosX + enemyBullet.Size / 2, enemyBullet.PosY + enemyBullet.Size / 2, 2); //Spawn in place of the object of "visual effects
        }

        public static void Interaction()
        {
            foreach (var enemyBullet in enemyBullets)
            {
                enemyBullet.Update();
                if (GameFunctional.startGame)
                {
                    //Collision of object and ship 
                    if (Ship.ship.Collision(enemyBullet))
                    {
                        MusicEffects.HitSound();
                        enemyBullet.Power = 0;
                        Ship.ship.EnergyLow(enemyBullet.Damage);
                        enemyBullet.DestroyingObject(enemyBullet);
                    }                   
                }
            }
            RemoveObjectFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectFromCollection()
        {
            enemyBullets.RemoveAll(item => item.Power <= 0);
            enemyBullets.RemoveAll(
                item => (item.PosX > GameFunctional.Width || item.PosX < 0 || item.PosY > GameFunctional.Height || item.PosY <0));
        }

        //Loading Stars
        static public void LoadObjects(int posX, int posY, int sizeH, int numberOfBullets)
        {       
            for (int ich = -numberOfBullets; ich <= numberOfBullets; ich++)
            {
                for (int jch = -numberOfBullets; jch <= numberOfBullets; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    if (Math.Abs(ich) + Math.Abs(jch) == numberOfBullets + 1 || (ich == 0 && Math.Abs(jch) == numberOfBullets) || (jch == 0 && Math.Abs(ich) == numberOfBullets))
                        enemyBullets.Add(new EnemyBullets(
                            new Point(posX + sizeH / 2 + ich * sizeH / 4, posY + sizeH / 2 + jch * sizeH / 4),
                            new Point(2 * ich, 2 * jch), new Size(20, 14)));
                }
            }
            MusicEffects.BossShotSound();
        }
    }
}
