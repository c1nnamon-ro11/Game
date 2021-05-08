using System.Drawing;

namespace FirstGame
{
    public class Ship : BaseObject
    {
        //Class spawn variables
        public static Ship ship = new Ship(new Point(30, 450), new Point(3, 3), new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT));

        //Image at screen
        static Image img = Image.FromFile("Content\\pictures\\ship.png");

        //Default object characteristics
        static readonly int DEFAULT_WIDTH = img.Width;
        static readonly int DEFAULT_HEIGHT = img.Height;

        //Fields and events of class
        public static event Message MessageDie; //Death 
        int energy = 300;                       //НР 
        int score = 0;                          //Player`s score
        protected int lvl = 2;                  //Ship lvl
        int bossTime = 0;                       //Score for spawn new boss target

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
        public void BossTimeUp(int number)
        {
            bossTime += number;
        }
        public int Lvl
        {
            set { lvl = value; }
            get { return lvl; }
        }
        public void ScoreUp(int number)
        {
            score += number;
        }
        public void EnergyLow(int number)
        {
            energy­ -= number;
        }
        public void LvlUp(int number)
        {
            Lvl += number;
            if (Lvl == 0) Lvl = 1;
            if (Lvl == 7)
            {
                Lvl = 6;
            }
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
            if (pos.Y > 0)
            {
                pos.Y = pos.Y­ - dir.Y;
            }
        }
        public void Down()
        {
            if (pos.Y + 80 < GameFunctional.Height)
            {
                pos.Y = pos.Y + dir.Y;
            }
        }
        public void Left()
        {
            if (pos.X > 0)
            {
                pos.X = pos.X - dir.X;
            }
        }
        public void Right()
        {
            if (pos.X + 50 < GameFunctional.Width)
            {
                pos.X = pos.X + dir.X;
            }
        }

        //Death
        public void Die()
        {
            if (MessageDie != null) MessageDie();
        }
    }
}
