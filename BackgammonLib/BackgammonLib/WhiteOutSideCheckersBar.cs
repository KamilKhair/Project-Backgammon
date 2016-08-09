using System.Linq;

namespace BackgammonLib
{
    internal class WhiteOutSideCheckersBar : OutSideCheckersBar
    {
        internal override void AddToBar(IBackgammon game, Checker checker)
        {
            var myGame = game as Backgammon;
            Bar.Push(checker);
            if (Bar.Count != 15) return;
            if (myGame == null)
            {
                return;
            }
            myGame.GameOver = true;
            myGame.Winner = CheckerType.White;
            myGame.RaiseGameFinishedEvent();
        }

        public override string ToString()
        {
            return Bar.Aggregate<Checker, string>(null, (current, checker) => current + " ()");
        }
    }
}
