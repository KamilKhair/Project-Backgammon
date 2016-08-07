using System;
using System.Linq;

namespace BackgammonLib
{
    public class Dice
    {
        internal Dice(Backgammon game)
        {
            _game = game;
        }

        public CheckerType CurrentType;

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
                if (!_game.BlackDeadCheckersBar.Any())
                {
                    return;
                }
                if (_game.GameBoard.Triangles[23 - FirstCube + 1].CheckersCount <= 1 ||
                    _game.GameBoard.Triangles[23 - SecondCube + 1].CheckersCount <= 1)
                {
                    return;
                }
                if (_game.GameBoard.Triangles[23 - FirstCube + 1].Type == CheckerType.White &&
                    _game.GameBoard.Triangles[23 - SecondCube + 1].Type == CheckerType.White)
                {
                    _game.Turn = CheckerType.White;
                }
            }
            else
            {
                if (!_game.WhiteDeadCheckersBar.Any())
                {
                    return;
                }
                if (_game.GameBoard.Triangles[FirstCube - 1].CheckersCount <= 1 ||
                    _game.GameBoard.Triangles[SecondCube - 1].CheckersCount <= 1)
                {
                    return;
                }
                if (_game.GameBoard.Triangles[FirstCube - 1].Type == CheckerType.Black &&
                    _game.GameBoard.Triangles[SecondCube - 1].Type == CheckerType.Black)
                {
                    _game.Turn = CheckerType.Black;
                }
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
                if (_game.IsWhitePlayerCanPlay)
                {
                    _game.Turn = CheckerType.White;
                }
            }
            else
            {
                if (_game.IsBlackPlayerCanPlay)
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
