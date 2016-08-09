using System;

namespace BackgammonLib
{
    public delegate void GameFinishedEventHandler(object sender, GameFinishedEventArgs e);

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(CheckerType winner)
        {
            Winner = winner;
        }
        public CheckerType Winner { get; set; }
    }
}