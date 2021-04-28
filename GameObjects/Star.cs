using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FirstGame
{
    public class Star : BaseObject
    {
        bool isObjectDestroyed;
        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\star.png");

        //Constructor with "power"
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
            Damage = 10;
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
                pos.Y = GameFunctional.rnd.Next(0, GameFunctional.Height);
            }
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y + Size > GameFunctional.Height) dir.Y = -dir.Y;
        }

        public static void Interaction(Star star, List<Bullet> bullets, List<VisualEffect> visualEffects, Ship ship, bool startGame, bool isBossFight, ref  int index)
        {
            star.Update();
            if (startGame)
            {
                //Collision of object and bullet  
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (bullets[j].Collision(star))
                    {
                        MusicEffects.HitSound();
                        star.PowerLow(bullets[j].Power);    //Object "power" reduction
                        if (star.Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                        {
                            /**/
                            ObjectLoader.VisualEffect(2,
                                star.PosX + star.Size / 2, star.PosY + star.Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                            ship.ScoreUp(star.Size);
                            if (!isBossFight)
                            {
                                ship.BossTimeUp(star.Size);
                            }
                            star.isObjectDestroyed = true;
                            /**/
                        }
                        bullets.RemoveAt(j);
                        j--;
                        index--;
                    }
                }
                //Collision of object and ship 
                if (ship.Collision(star))
                {
                    MusicEffects.HitSound();                    
                    ship.EnergyLow(star.Damage);
                    if (ship.Energy <= 0)
                    {
                        ship.Die();
                    }
                    /**/
                    ObjectLoader.VisualEffect(2,
                        star.PosX + star.Size / 2, star.PosY + star.Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                    ship.ScoreUp(star.Size);
                    if (!isBossFight)
                    {
                        ship.BossTimeUp(star.Size);
                    }
                    star.isObjectDestroyed = true;
                    /**/
                }
            }
        }

        public static void RemoveObjectFromCollection(List<Star> gameObjects)
        {
            //foreach(Star gameObject in gameObjects)
            {
                gameObjects.RemoveAll(item => item.isObjectDestroyed);
            }
        }        
    }
}
