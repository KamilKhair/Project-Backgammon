using System.Collections.Generic;
namespace BackgammonLib
{
    internal class OutSideCheckersBar
    {
        internal Stack<Checker> Bar = new Stack<Checker>();

        internal virtual void AddToBar(IBackgammon game, Checker checker)
        {
            
        }
    }
}
