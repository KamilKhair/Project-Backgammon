namespace BackgammonLib
{
    public class Checker : IChecker
    {
        internal Checker(CheckerType t, int triangle, int id, bool alive = true, bool finished = false)
        {
            Type = t;
            IsAlive = alive;
            IsFinished = finished;
            CheckerTriangle = triangle;
            CheckerId = id;
        }

        public CheckerType Type { get; internal set; }

        public int CheckerTriangle { get; internal set; }

        public int CheckerId { get; internal set; }

        public bool IsAlive { get; internal set; }

        public bool IsFinished { get; internal set; }
    }
}
