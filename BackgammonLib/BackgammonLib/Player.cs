namespace BackgammonLib
{
    public class Player : IPlayer
    {
        public Player(CheckerType type, Backgammon game)
        {
            Type = type;
            Board = game.Board;
            Dice = game.Dice;
            Game = game;
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
        protected readonly Dice Dice;
        protected readonly Backgammon Game;
        internal DeadCheckersBar DeadCheckersBar;
        internal OutSideCheckersBar OutSideCheckersBar;
    }
}