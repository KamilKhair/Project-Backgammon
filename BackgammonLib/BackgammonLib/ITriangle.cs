using System.Collections.Generic;

namespace BackgammonLib
{
    public interface ITriangle
    {
        CheckerType Type { get; }
        bool IsEmpty { get; }
        int CheckersCount { get; }
        int TriangleId { get; }
        Stack<IChecker> CheckersStack { get; }
    }
}
