using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class DeadCheckersBar
    {
        internal Stack<Checker> Bar = new Stack<Checker>();
        internal virtual void AddToBar(Checker checker)
        {
            
        }

        internal virtual Checker RemoveFromBar()
        {
            return null;
        }
    }
}
