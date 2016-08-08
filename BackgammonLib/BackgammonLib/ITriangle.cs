using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
