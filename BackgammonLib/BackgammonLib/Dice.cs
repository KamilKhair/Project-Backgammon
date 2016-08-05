using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgammonLib
{
    public class Dice
    {
        public CheckerType CurrentType;

        public int Steps { get; private set; }

        public int FirstCube { get; private set; }

        public int SecondCube { get; private set; }

        public bool RolledDouble { get; private set; }

        private static readonly Random Rand = new Random();

        internal bool RollDice(Backgammon game, CheckerType type)
        {
            if (Steps != 0) return false;
            FirstCube = Rand.Next(1, 7);
            SecondCube = Rand.Next(1, 7);
            RolledDouble = FirstCube == SecondCube;
            Steps = RolledDouble ? 4 : 2;
            CurrentType = type;
            UpdateTurn(game);
            return true;
        }

        private void UpdateTurn(Backgammon game)
        {
            if (game.Turn == CheckerType.Black)
            {
                if (game.BlackDeadCheckersBar.Any())
                {
                    if (game._board.Triangles[23 - FirstCube + 1].CheckersCount > 1 && game._board.Triangles[23 - SecondCube + 1].CheckersCount > 1)
                    {
                        if (game._board.Triangles[23 - FirstCube + 1].Type == CheckerType.White &&
                            game._board.Triangles[23 - SecondCube + 1].Type == CheckerType.White)
                        {
                            game.Turn = CheckerType.White;
                            return;
                        }
                    }
                }
            }
            else
            {
                if (game.WhiteDeadCheckersBar.Any())
                {
                    if (game._board.Triangles[FirstCube - 1].CheckersCount > 1 && game._board.Triangles[SecondCube - 1].CheckersCount > 1)
                    {
                        if (game._board.Triangles[FirstCube - 1].Type == CheckerType.Black &&
                            game._board.Triangles[SecondCube - 1].Type == CheckerType.Black)
                        {
                            game.Turn = CheckerType.Black;
                            return;
                        }
                    }
                }
            }
        }

        internal void DecrementSteps(Backgammon game)
        {
            if (Steps > 0)
            {
                --Steps;
            }
            if (Steps != 0) return;
            if (game.Turn == CheckerType.Black)
            {
                if (game.IsWhitePlayerCanPlay)
                {
                    game.Turn = CheckerType.White;
                }
            }
            else
            {
                if (game.IsBlackPlayerCanPlay)
                {
                    game.Turn = CheckerType.Black;
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
