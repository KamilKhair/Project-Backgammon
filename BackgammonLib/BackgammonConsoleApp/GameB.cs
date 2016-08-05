using System;

namespace BackgammonConsoleApp
{
    internal struct MyGameBoard
    {
        public MyGameBoard(string value)
        {
            FgColor = ConsoleColor.Black;
            Value = value;
        }

        public string Value { get; set; }
        public ConsoleColor FgColor { get; set; }
    }
}