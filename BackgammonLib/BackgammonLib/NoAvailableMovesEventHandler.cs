using System;

namespace BackgammonLib
{
    public delegate void NoAvailableMovesEventHandler(object sender, NoAvailableMovesEventArgs e);

    public class NoAvailableMovesEventArgs : EventArgs
    {
        public NoAvailableMovesEventArgs(int first, int second)
        {
            FirstCube = first;
            SecondCube = second;
        }
        public int FirstCube { get; set; }
        public int SecondCube { get; set; }
    }
}