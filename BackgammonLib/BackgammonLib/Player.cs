namespace BackgammonLib
{
    internal class Player
    {
        internal Player(CheckerType type, GameBoard board, Dice dice, Backgammon game)
        {
            Type = type;
            Board = board;
            Dice = dice;
            Game = game;
        }

        internal CheckerType Type;
        protected readonly GameBoard Board;
        protected readonly Dice Dice;
        protected readonly Backgammon Game;

        internal DeadCheckersBar DeadCheckersBar;
        internal OutSideCheckersBar OutSideCheckersBar;

        internal virtual bool Roll()
        {
            return true;
        }

        internal virtual bool Move(int triangle, int move)
        {
            return true;
        }

        internal virtual bool MoveFromDeadBar(int move)
        {
            return true;
        }

    }
}