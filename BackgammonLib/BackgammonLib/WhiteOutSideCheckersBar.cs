using System.Linq;

namespace BackgammonLib
{
    internal class WhiteOutSideCheckersBar : OutSideCheckersBar
    {
        internal override void AddToBar(Backgammon game, Checker checker)
        {
            Bar.Push(checker);
            if (Bar.Count != 15) return;
            game.GameOver = true;
            game.Winner = CheckerType.White;
            game.RaiseGameFinishedEvent();
        }

        public override string ToString()
        {
            return Bar.Aggregate<Checker, string>(null, (current, checker) => current + " ()");
        }
    }
}
