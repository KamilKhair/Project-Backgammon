using System;
namespace BackgammonLib
{
    public class Dice : IDice
    {
        internal Dice(Backgammon game)
        {
            _game = game;
        }

        public CheckerType CurrentType { get; private set; }
        public int Steps { get; private set; }

        public int FirstCube { get; private set; }

        public int SecondCube { get; private set; }

        public bool RolledDouble { get; private set; }

        private static readonly Random Rand = new Random();

        private readonly Backgammon _game;

        internal bool RollDice(CheckerType type)
        {
            if (Steps != 0) return false;
            FirstCube = Rand.Next(1, 7);
            SecondCube = Rand.Next(1, 7);
            RolledDouble = FirstCube == SecondCube;
            Steps = RolledDouble ? 4 : 2;
            CurrentType = type;
            UpdateTurn();
            return true;
        }

        private void UpdateTurn()
        {
            if (_game.Turn == CheckerType.Black)
            {
                if (_game.BlackPlayer.CheckIfThereAreAvailableMoves())
                {
                    return;
                }
                _game.Turn = CheckerType.White;
                _game.RaiseNoAvailableMovesEvent(FirstCube, SecondCube);
                ResetDice();
            }
            else
            {
                if (_game.WhitePlayer.CheckIfThereAreAvailableMoves())
                {
                    return;
                }
                _game.Turn = CheckerType.Black;
                _game.RaiseNoAvailableMovesEvent(FirstCube, SecondCube);
                ResetDice();
            }
        }

        internal void DecrementSteps()
        {
            if (Steps > 0)
            {
                --Steps;
            }
            if (Steps != 0) return;
            if (_game.Turn == CheckerType.Black)
            {
                if (_game.WhitePlayer.IsWhitePlayerCanPlay)
                {
                    _game.Turn = CheckerType.White;
                }
            }
            else
            {
                if (_game.BlackPlayer.IsBlackPlayerCanPlay)
                {
                    _game.Turn = CheckerType.Black;
                }
            }
        }

        internal void ResetFirstCube()
        {
            FirstCube = 0;
        }

        internal void ResetSecondCube()
        {
            SecondCube = 0;
        }

        internal void ResetDice()
        {
            RolledDouble = false;
            Steps = 0;
            ResetFirstCube();
            ResetSecondCube();
        }
    }
}
