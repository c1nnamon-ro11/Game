using System.Drawing;

namespace FirstGame
{
    public class Ship : BaseObject
    {
        //Fields and events of class
        public static event Message MessageDie; //Death 
        int energy = 300;                       //НР 
        int score = 0;                          //Player`s score
        protected int lvl = 2;                  //Ship lvl
        int bossTime = 0;                       //Score for spawn new boss target

        //Image at screen
        Image img = Image.FromFile("Content\\pictures\\ship.png");

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
            if (pos.Y + 80 < GameFunctional.Height) pos.Y = pos.Y + dir.Y;
        }
        public void Left()
        {
            if (pos.X > 0) pos.X = pos.X - dir.X;
        }
        public void Right()
        {
            if (pos.X + 50 < GameFunctional.Width) pos.X = pos.X + dir.X;
        }

        //Death
        public void Die()
        {
            if (MessageDie != null) MessageDie();
        }
    }
}
