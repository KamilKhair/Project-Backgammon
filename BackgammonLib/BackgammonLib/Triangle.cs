using System.Collections.Generic;

namespace BackgammonLib
{
    public class Triangle : ITriangle
    {
        internal Triangle(CheckerType t, int triangle, int initialId, bool empty = true, int count = 0)
        {
            CheckersStack = new Stack<IChecker>();
            Type = t;
            IsEmpty = empty;
            CheckersCount = count;
            TriangleId = triangle;
            if (!IsEmpty)
            {
                InitializeStack(t, TriangleId, CheckersCount, initialId);
            }
        }

        private void InitializeStack(CheckerType t, int triangle, int count, int initialId)
        {
            for (var i = 0; i < count; ++i)
            {
                CheckersStack.Push(new Checker(t, triangle, initialId++));
            }
        }

        public CheckerType Type { get; internal set; }
        public bool IsEmpty { get; internal set; }
        public int CheckersCount { get; internal set; }
        public int TriangleId { get; internal set; }
        public Stack<IChecker> CheckersStack { get; internal set; }
    }
}