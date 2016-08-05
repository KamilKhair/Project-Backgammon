using System.Collections.Generic;

namespace BackgammonLib
{
    internal class Player
    {
        internal Player(string name, CheckerType type)
        {
            PlayerName = name;
            Type = type;
        }

        internal string PlayerName;

        internal CheckerType Type;

        internal DeadCheckersBar _deadCheckersBar;
        internal OutSideCheckersBar _outSideCheckersBar;

        internal virtual bool Roll(Backgammon game)
        {
            return true;
        }

        internal virtual bool Move(Backgammon game, int triangle, int move)
        {
            return true;
        }

        internal virtual bool MoveFromDeadBar(Backgammon game, int move)
        {
            return true;
        }

    }
}