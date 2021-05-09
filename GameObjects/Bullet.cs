using System;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    public class Bullet : Ship
    {
        //Class spawn variables
        public static List<Bullet> bullets = new List<Bullet>();
        private static Random rnd = new Random();
        private bool isSuperBullet;

        //Images at screen
        Image defaultBullet = Image.FromFile(GameFunctional.texturePackPath + "bullet1.png");
        Image upgradeBullet = Image.FromFile(GameFunctional.texturePackPath + "bullet2.png");

        //Overloading (2 types of bullets)
        public override int Power
        {
            get { return power; }
            set { power = value; }
        }

        //Constructor
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
        }

        public Bullet(Point pos, Point dir, Size size, bool isSuperBullet) : base(pos, dir, size)
        {            
            this.isSuperBullet = isSuperBullet;
            if (isSuperBullet)
            {
                Power = 20;
            }
            else
            {
                Power = 10;
            }
        }

        //Drawing at game screen
        public override void Drawing()
        {
            if (isSuperBullet)
            {
                GameFunctional.buffer.Graphics.DrawImage(upgradeBullet, pos);
            } 
            else GameFunctional.buffer.Graphics.DrawImage(defaultBullet, pos);
        }

        //Calculating new position of object
        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
        }

        //Procedure before removing game object from gamescreen
        public static void DestroyingObject(Bullet bullet)
        {
            bullet.Power = 0;
        }

        //Functional logic of Ship bullets
        public static void Interaction()
        {
            foreach (var bullet in bullets)
            {
                if (GameFunctional.startGame)
                {
                    bullet.Update();
                    if (bullet.PosX > GameFunctional.Width)
                    {
                        DestroyingObject(bullet);
                    }
                }
            }
            RemoveObjectFromCollection();
        }

        //Removing objects from gamescreen
        public static void RemoveObjectFromCollection()
        {           
            bullets.RemoveAll(item => item.Power <= 0);         
        }

        //Loading Bullets
        static public void LoadObjects(int posX, int posY, int sizeH, int numberOfBullets)
        {           
        }
    }
}
