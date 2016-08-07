namespace BackgammonLib
{
    public enum CheckerType
    {
        Black,
        White,
        None
    }

    public class Checker
    {
        internal Checker(CheckerType t, int triangle, int id, bool alive = true, bool finished = false)
        {
            Type = t;
            IsAlive = alive;
            IsFinished = finished;
            CheckerTriangle = triangle;
            CheckerId = id;
        }
        internal CheckerType Type;
        internal int CheckerTriangle;
        internal int CheckerId;
        internal bool IsAlive;
        internal bool IsFinished;

        public CheckerType CheckerType => Type;
        public int CheckerNumber => CheckerId;
        public int CheckerTraingle => CheckerTriangle;
    }
}
