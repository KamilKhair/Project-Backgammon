using System.Linq;

namespace BackgammonLib
{
    internal class BlackDeadCheckersBar : DeadCheckersBar
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
