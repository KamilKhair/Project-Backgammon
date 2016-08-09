using System;

namespace BackgammonLib
{
    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(CheckerType winner)
        {
            Winner = winner;
        }
        public CheckerType Winner { get; set; }
    }
}