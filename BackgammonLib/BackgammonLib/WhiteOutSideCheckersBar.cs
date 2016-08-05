using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class WhiteOutSideCheckersBar : OutSideCheckersBar
    {
        internal override void AddToBar(Backgammon game, Checker checker)
        {
            Bar.Push(checker);
            if (Bar.Count != 15) return;
            game.GameOver = true;
            game._winner = CheckerType.White;
        }

        public override string ToString()
        {
            return Bar.Aggregate<Checker, string>(null, (current, checker) => current + " ()");
        }
    }
}
