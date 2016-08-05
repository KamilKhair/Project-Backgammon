using System.Collections.Generic;

namespace BackgammonLib
{
    public class Triangle
    {
        internal Triangle(CheckerType t, int triangle, int initialId, bool empty = true, int count = 0)
        {
            Type = t;
            IsEmpty = empty;
            CheckersCount = count;
            TriangleId = triangle;
            if (!IsEmpty)
            {
                InitializeStack(t, TriangleId, CheckersCount, initialId);
            }
        }

        private void InitializeStack(CheckerType t, int triangle,  int count, int initialId)
        {
            for (var i = 0; i < count; ++i)
            {
                CheckersStack.Push(new Checker(t, triangle, initialId++));
            }
        }

        internal CheckerType Type;
        internal bool IsEmpty;
        internal int CheckersCount;
        internal int TriangleId;
        internal Stack<Checker> CheckersStack = new Stack<Checker>();

        public CheckerType TriangleType => Type;
        public int TrianglrId => TriangleId;
        public IEnumerable<Checker> Checkers => CheckersStack;
    }
}