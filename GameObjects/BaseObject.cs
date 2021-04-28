using System.Drawing;

namespace FirstGame
{
    public delegate void Message();

    //Interface collision
    public interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    public abstract class BaseObject:ICollision
    {
        //Protected fields 
        protected Point pos; //Coor of object
        protected Point dir; //"speed" of object
        protected Size size; //object size
        protected int power; //object "power"
        protected int damage; //objetr "damage"

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

        public virtual int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        //Methods
        public void PowerLow(int n)
        {
            { power­ -= n; }
        }

        abstract public void Drawing();

        abstract public void Update();

        //abstract public void Interaction();

        public bool Collision(ICollision o)
        {
            if (o.Rect.IntersectsWith(this.Rect)) return true; else return false;
        }

        public Rectangle Rect
        {
            get { return new Rectangle(pos, size); }
        }
    }
}

