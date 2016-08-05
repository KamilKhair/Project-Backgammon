using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class OutSideCheckersBar
    {
        internal Stack<Checker> Bar = new Stack<Checker>();

        internal virtual void AddToBar(Backgammon game, Checker checker)
        {
            
        }
    }
}
