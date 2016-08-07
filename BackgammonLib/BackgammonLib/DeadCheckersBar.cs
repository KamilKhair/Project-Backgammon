using System.Collections.Generic;

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
