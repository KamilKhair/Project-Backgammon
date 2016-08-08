using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    public interface IChecker
    {
        CheckerType Type { get; }
        int CheckerTriangle { get; }
        int CheckerId { get; }
        bool IsAlive { get; }
        bool IsFinished { get; }
    }
}
