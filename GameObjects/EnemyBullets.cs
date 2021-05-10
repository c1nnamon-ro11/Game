using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    class EnemyBullets : BaseObject
    {
        //Class spawn variables
        public static List<EnemyBullets> enemyBullets = new List<EnemyBullets>();
        private static Random rnd = new Random();

        //Image at screen
        static Image img = Image.FromFile(GameFunctional.texturePackPath + "enemyBullet.png");

        //Default object characteristics
        const int DEFAULT_POWER = 100;
        const int DEFAULT_DAMAGE = 15;
        readonly int DEFAULT_WIDTH = img.Width;
        readonly int DEFAULT_HEIGHT = img.Height;

        //Constructors
        public EnemyBullets(Point pos, Point dir) : base(pos, dir)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public EnemyBullets(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = DEFAULT_POWER;
            damage = DEFAULT_DAMAGE;
        }

        public EnemyBullets(Point pos, Point dir, int power, int damage) : base(pos, dir, power, damage)
        {
            size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public EnemyBullets(Point pos, Point dir, Size size, int power, int damage) : base(pos, dir, size, power, damage)
        {
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

        //Procedure before removing game object from gamescreen
        private void DestroyingObject(EnemyBullets enemyBullet)
        {
            VisualEffect.LoadObjects(
                            enemyBullet.PosX + enemyBullet.WidthSize / 2, enemyBullet.PosY + enemyBullet.HeightSize / 2, 2); //Spawn in place of the object of "visual effects
        }

        //Functional logic of EnemyBullets
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
                item => 
                (item.PosX > GameFunctional.Width || item.PosX+item.WidthSize < 0 ||
                item.PosY - item.HeightSize > GameFunctional.Height || item.PosY < 0));
        }

        //Loading EnemyBullets
        static public void LoadObjects(int posX, int posY, int posSizeWidth, int posSizeHeigth, int numberOfBullets)
        {       
            for (int ich = -numberOfBullets; ich <= numberOfBullets; ich++)
            {
                for (int jch = -numberOfBullets; jch <= numberOfBullets; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    if (Math.Abs(ich) + Math.Abs(jch) == numberOfBullets + 1 || (ich == 0 && Math.Abs(jch) == numberOfBullets) || (jch == 0 && Math.Abs(ich) == numberOfBullets))
                        enemyBullets.Add(new EnemyBullets(                            
                            new Point(
                                posX + posSizeWidth / 2 + ich * posSizeWidth / 4 - img.Width/2,
                                posY + posSizeHeigth / 2 + jch * posSizeHeigth / 4 - img.Height/2),
                            new Point(2 * ich, 2 * jch)));
                }
            }
            MusicEffects.BossShotSound();
        }
    }
}
