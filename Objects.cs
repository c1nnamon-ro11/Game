using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FirstGame
{
    delegate void Message();
    //Interface collision
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    //Objects descriptions 

    //Base object
    //=================================================================================================================
    abstract class BaseObject:ICollision
    {
        //Protected fields 
        protected Point pos; //Coor of object
        protected Point dir; //"speed" of object
        protected Size size; //object size
        protected int power; //objecr "power"

        //Constructor
        public BaseObject(Point pos, Point dir, Size size)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
        }
        //Properties 
        public int PosX
        {
            set { if(value>0)pos.X = value; }
            get { return pos.X; }
        }
        public int PosY
        {
            set { if (value > 0) pos.Y = value; }
            get { return pos.Y; }
        }
        public int Size
        {
            get { return size.Height; }
        }

        public virtual int Power
        {
            get { return power; }
            set { power = value; }
        }
        //Methods
        public void PowerLow(int n)
        {
            { power­ -= n; }
        }

        abstract public void Drawing();

        abstract public void Update();

        public bool Collision(ICollision o)
        {
            if (o.Rect.IntersectsWith(this.Rect)) return true; else return false;
        }
        public Rectangle Rect
        {
            get { return new Rectangle(pos, size); }
        }
    }

    //Inherited classes (objects)

    //Star
    //=================================================================================================================
    class Star : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\star.png");

        //Constructor with "power"
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
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
            if (pos.X < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = GameFunctional.rnd.Next(0, GameFunctional.Height);
            }
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y > GameFunctional.Height) dir.Y = -dir.Y;
        }
    }

    //Rocket
    //=================================================================================================================
    class Rocket : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\rocket.png");

        //Constructor with "power"
        public Rocket(Point pos,Point dir, Size size):base(pos,dir,size)
        {
            Power = 5;
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
            if (pos.X < 0)
            {
                pos.X = GameFunctional.Width+10;
                pos.Y = GameFunctional.rnd.Next(0, GameFunctional.Height);
                dir.X = -GameFunctional.rnd.Next(10, 15);
            }
        }
    }
    //Астероїд
    //=================================================================================================================
    class Asteroid : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\asteroid.png");

        //Constructor with "power"
        public Asteroid(Point pos,Point dir,Size size):base(pos,dir,size)
        {
            Power = 50;
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
            if (pos.X < 0)
            {
                pos.X = GameFunctional.Width + 10;
                pos.Y = GameFunctional.rnd.Next(0, GameFunctional.Height);
            }
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y > GameFunctional.Height) dir.Y = -dir.Y;
        }
    }

    //Уламки астероїду
    //=================================================================================================================
    class AsteroidCharge : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\charge.png");
        //Constructor with "power"
        public AsteroidCharge(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 5;
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
            if (dir.X==0 && dir.Y==0)
            {
                dir.X = GameFunctional.rnd.Next(-1, 13);
                dir.Y = GameFunctional.rnd.Next(-4,10);
            }
        }
    }

    //Game ship
    //=================================================================================================================
    class Ship : BaseObject
    {
        //field and events of class
        public static event Message MessageDie; //Death 
        int energy = 300;                       //НР 
        int score = 0;                          //Player`s score
        protected int lvl = 2;                  //Ship lvl
        int bossTime=0;                         //Score for spawn new boss target

        //Image at screen
        Image img = Image.FromFile("pictures\\ship.png");

        //Properties (HP,Score, Boss score)
        public int Energy
        {
            get { return energy; }
            set { energy = value; }
        }
        public int Score
        {
            get { return score; }
        }
        public int BossTime
        {
            get { return bossTime; }
            set { bossTime = value; }
        }

        //Methods for caltulating the characterics of game ship
        public void BossTimeUp(int n)
        {
            bossTime += n; 
        }
        public int Lvl
        {
            set { lvl = value; }
            get { return lvl; }
        }
        public void ScoreUp(int n)
        {
            score += n;            
        }
        public void EnergyLow(int n)
        {
             energy­ -= n; 
        }
        public void LvlUp(int n)
        {
            lvl += n;
            if (lvl == 0) lvl = 1;
        }

        //Empty constructor
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        //Drawing at game screen
        override public void Drawing()
        {
            GameFunctional.buffer.Graphics.DrawImage
                (img, pos);
        }

        //Ship control
        public override void Update()
        {
        }
        public void Up()
        {
            if (pos.Y > 0) pos.Y = pos.Y­ - dir.Y;
        }
        public void Down()
        {
            if (pos.Y < GameFunctional.Height) pos.Y = pos.Y + dir.Y;
        }
        public void Left()
        {
            if(pos.X>0) pos.X = pos.X - dir.X;
        }
        public void Right()
        {
            if (pos.X < GameFunctional.Width) pos.X = pos.X + dir.X;
        }

        //Death
        public void Die()
        {
            if (MessageDie != null) MessageDie();
        }
    }

    //Bullet
    //=================================================================================================================
    class Bullet : Ship
    {
        //Images at screen
        Image img1 = Image.FromFile("pictures\\bullet1.png");
        Image img2 = Image.FromFile("pictures\\bullet2.png");

        //Overloading (because we have 2 types of bullets)
        public override int Power
        {
            get { return power; }
            set { if (lvl > 3) power = 50; else power = value; }
        }

        //Constructor
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
        }
        //Drawing at game screen
        public override void Drawing()
        {
            if (lvl <= 3) GameFunctional.buffer.Graphics.DrawImage (img1, pos);
            else GameFunctional.buffer.Graphics.DrawImage(img2, pos);
        }
        //Calculating new position of object
        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
        }
    }

    //Bonus
    //=================================================================================================================
    class BonusUp : Ship
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\bonus.png");
        //Constructor
        public BonusUp(Point pos, Point dir, Size size) : base(pos, dir, size)
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
            pos.Y = pos.Y + dir.Y;
            if (pos.Y < Size) dir.Y = -dir.Y;
            if (pos.Y > GameFunctional.Height) dir.Y = -dir.Y;
        }
    }

    //Boss
    //=================================================================================================================
    class Boss : BaseObject
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\boss.png");
        //Constructor
        public Boss(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            power = 2000;
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

            if (pos.X < GameFunctional.Width / 2) dir.X= GameFunctional.rnd.Next(2, 5); 
            if (pos.X > GameFunctional.Width - 350) dir.X = -GameFunctional.rnd.Next(2, 5);
            if (pos.Y < 0) dir.Y = GameFunctional.rnd.Next(2, 5);
            if (pos.Y > GameFunctional.Height - 350) dir.Y = -GameFunctional.rnd.Next(2, 5);
        }
    }

    //Enemy bullets
    //=================================================================================================================
    class EnemyBullets : Boss
    {
        //Image at screen
        Image img = Image.FromFile("pictures\\fireball2.png");
        //Constructor
        public EnemyBullets(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 10;
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
    }

    //"Visua effects" when object destoyed
    //=================================================================================================================
    class VisualEffect : BaseObject
    {
        //Constructor
        public VisualEffect(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        //Drawing at game screen
        public override void Drawing()
        {
            GameFunctional.buffer.Graphics.FillEllipse(
                Brushes.Aquamarine, pos.X, pos.Y, size.Width, size.Height);
        }

        //Calculating new position of object
        override public void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y = pos.Y + dir.Y;
            if (dir.X == 0 && dir.Y == 0)
            {
                dir.X = GameFunctional.rnd.Next(-5, 5);
                dir.Y = GameFunctional.rnd.Next(-5, 5);

            }
        }
    }
}

