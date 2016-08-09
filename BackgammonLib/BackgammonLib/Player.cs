namespace BackgammonLib
{
    public class Player : IPlayer
    {
        public Player(CheckerType type, IBackgammon game)
        {
            Type = type;
            Board = game.Board as GameBoard;
            Dice = game.Dice as Dice;
            Game = game as Backgammon;
        }

        public virtual bool IsWhitePlayerCanPlay { get; set; }
        public virtual bool IsBlackPlayerCanPlay { get; set; }

        public virtual bool Roll()
        {
            return true;
        }

        public virtual bool Move(int triangle, int move)
        {
            return true;
        }

        public virtual bool MoveFromDeadBar(int move)
        {
            return true;
        }

        public virtual bool CheckIfThereAreAvailableMoves()
        {
            return true;
        }

        public virtual bool CheckIfCanMove(int triangle, int move)
        {
            return true;
        }

        public virtual bool CheckIfCanMoveFromDeadBar(int triangle)
        {
            return true;
        }

        internal CheckerType Type;
        protected readonly IGameBoard Board;
        protected readonly IDice Dice;
        protected readonly IBackgammon Game;
        internal DeadCheckersBar DeadCheckersBar;
        internal OutSideCheckersBar OutSideCheckersBar;
    }
}