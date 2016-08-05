using System;

namespace BackgammonConsoleApp
{
    internal struct GameB
    {
        public GameB(string value)
        {
            FgColor = ConsoleColor.Black;
            Value = value;
        }

        public string Value { get; set; }
        public ConsoleColor FgColor { get; set; }
    }
}