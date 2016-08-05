using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class WhiteDeadChechersBar : DeadCheckersBar
    {
        internal override void AddToBar(Checker checker)
        {
            Bar.Push(checker);
        }

        internal override Checker RemoveFromBar()
        {
            return Bar.Pop();
        }

        public override string ToString()
        {
            return Bar.Aggregate<Checker, string>(null, (current, checker) => current + " ()");
        }
    }
}
