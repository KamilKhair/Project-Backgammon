using System.Threading;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class BlackPlayer : Player
    {
        internal BlackPlayer(CheckerType t, Backgammon game) : base(t, game)
        {
            DeadCheckersBar = new BlackDeadCheckersBar();
            OutSideCheckersBar = new BlackOutSideCheckersBar();
        }

        internal override bool Move(int triangle, int move)
        {
            if (!CheckIfThereAreAvailableMoves())
            {
                return false;
            }
            if (!CheckIfCanMove(triangle, move))
            {
                return false;
            }
            if (triangle <= move)
            {
                MoveToOutsideBar(triangle, move);
            }
            else
            {
                PerformMove(triangle, move);
            }
            UpdateDice(move);
            if (!CheckIfThereAreAvailableMoves())
            {
                Game.Turn = CheckerType.White;
                Dice.ResetSecondCube();
            }
            return true;
        }

        private void PerformMove(int triangle, int move)
        {
            var destinationTriangle = Board.Triangles[triangle - 1 - move];
            var sourceTriangle = Board.Triangles[triangle - 1];
            var sourceChecker = sourceTriangle.CheckersStack.Pop();

            //Option 1: Moving to a triangle which has only one white checker.
            if (destinationTriangle.Type == CheckerType.White && destinationTriangle.CheckersCount == 1)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead;
                destinationChecker.IsAlive = false;
                Game.WhitePlayer.DeadCheckersBar.AddToBar(destinationChecker);
                destinationTriangle.CheckersStack.Push(sourceChecker);
                destinationTriangle.Type = CheckerType.Black;
            }
            //Option 2: Moving to an empty triangle or a triangle which has at least one black checker.
            else if (destinationTriangle.Type == CheckerType.Black || destinationTriangle.Type == CheckerType.None)
            {
                ++destinationTriangle.CheckersCount;
                destinationTriangle.CheckersStack.Push(sourceChecker);
                if (destinationTriangle.IsEmpty)
                {
                    destinationTriangle.IsEmpty = false;
                    destinationTriangle.Type = CheckerType.Black;
                }
                if (sourceTriangle.CheckersCount == 0)
                {
                    sourceTriangle.Type = CheckerType.None;
                    sourceTriangle.IsEmpty = true;
                }
            }
            sourceChecker.CheckerTriangle -= move;
            --sourceTriangle.CheckersCount;
            if (sourceTriangle.CheckersCount == 0)
            {
                sourceTriangle.Type = CheckerType.None;
                sourceTriangle.IsEmpty = true;
            }
        }

        internal override bool MoveFromDeadBar(int triangle)
        {
            if (!CheckIfThereAreAvailableMoves())
            {
                return false;
            }
            if (!CheckIfCanMoveFromDeadBar(triangle))
            {
                return false;
            }
            var destinationTriangle = Board.Triangles[triangle - 1];
            var sourceChecker = DeadCheckersBar.RemoveFromBar();
            sourceChecker.IsAlive = true;
            sourceChecker.CheckerTriangle = triangle - 1;

            //Option 1: Moving to a triangle which has only one white checker.
            if (destinationTriangle.CheckersCount == 1 && destinationTriangle.Type == CheckerType.White)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead
                destinationChecker.IsAlive = false;
                Game.WhitePlayer.DeadCheckersBar.Bar.Push(destinationChecker);
                destinationTriangle.CheckersStack.Push(sourceChecker);
                destinationTriangle.Type = CheckerType.Black;
                UpdateDiceAfterMoveFromDeadBar(triangle);
                if (!CheckIfThereAreAvailableMoves())
                {
                    Game.Turn = CheckerType.White;
                    Dice.ResetSecondCube();
                }
                return true;
            }

            //Option 2: Moving to an empty triangle or a triangle which has at least one black checker.
            ++destinationTriangle.CheckersCount;
            destinationTriangle.CheckersStack.Push(sourceChecker);
            if (destinationTriangle.IsEmpty)
            {
                destinationTriangle.IsEmpty = false;
                destinationTriangle.Type = CheckerType.Black;
            }

            UpdateDiceAfterMoveFromDeadBar(triangle);
            if (!CheckIfThereAreAvailableMoves())
            {
                Game.Turn = CheckerType.White;
                Dice.ResetSecondCube();
            }
            return true;
        }

        private void MoveToOutsideBar(int triangle, int move)
        {
            var sourceTriangle = Board.Triangles[triangle - 1];
            var sourceChecker = sourceTriangle.CheckersStack.Pop();
            sourceChecker.CheckerTriangle = -2; // -2 = outside
            sourceChecker.IsFinished = true;
            sourceChecker.IsAlive = false;
            OutSideCheckersBar.AddToBar(Game, sourceChecker);
            --sourceTriangle.CheckersCount;
            if (sourceTriangle.CheckersCount != 0) return;
            sourceTriangle.Type = CheckerType.None;
            sourceTriangle.IsEmpty = true;
        }

        internal override bool Roll()
        {
            return Dice.Steps == 0 && Dice.RollDice(CheckerType.Black);
        }

        internal override bool CheckIfCanMove(int triangle, int move)
        {
            if (Game.Turn == CheckerType.White)
            {
                return false;
            }
            if (move < 1 || move > 6 || triangle < 1 || triangle > 24)
            {
                return false;
            }
            if (move != Dice.FirstCube && move != Dice.SecondCube)
            {
                return false;
            }
            if (AllCheckersInLocalArea(6, 24))
            {

            }
            else
            {
                var destinationTriangLe = triangle - move - 1;
                if (destinationTriangLe < 0 || destinationTriangLe > 23)
                {
                    return false;
                }
                if (Board.Triangles[destinationTriangLe].Type == CheckerType.White &&
                    Board.Triangles[destinationTriangLe].CheckersCount > 1)
                {
                    return false;
                }
            }

            return true;
        }

        internal override bool CheckIfCanMoveFromDeadBar(int triangle)
        {
            if (Game.Turn == CheckerType.White)
            {
                return false;
            }
            if (triangle < 1 || triangle > 24)
            {
                return false;
            }
            if (triangle != 24 - Dice.FirstCube + 1 && triangle != 24 - Dice.SecondCube + 1)
            {
                return false;
            }
            var destinationTriangLe = triangle - 1;
            if (destinationTriangLe < 0 || destinationTriangLe > 23)
            {
                return false;
            }
            if (Board.Triangles[destinationTriangLe].Type == CheckerType.White &&
                Board.Triangles[destinationTriangLe].CheckersCount > 1)
            {
                return false;
            }
            return true;
        }


        internal override bool CheckIfThereAreAvailableMoves()
        {
            if (ThereAreAnyavailableMoves())
            {
                return true;
            }
            if (ThereAreAnyavailableMovesFromDeadBar())
            {
                return true;
            }
            if (AllCheckersInLocalArea(6, 24))
            {
                return true;
            }
            return false;
        }

        private bool ThereAreAnyavailableMoves()
        {
            var canMove = false;
            if (!AllCheckersInLocalArea(6, 24) && DeadCheckersBar.Bar.Count == 0)
            {
                Parallel.For(0, 24, (i) =>
                {
                    var destinationTriangle1 = int.MinValue;
                    var destinationTriangle2 = int.MinValue;
                    if (Dice.FirstCube != 0)
                    {
                        destinationTriangle1 = i - Dice.FirstCube - 1;
                    }
                    if (Dice.SecondCube != 0)
                    {
                        destinationTriangle2 = i - Dice.SecondCube - 1;
                    }
                    if (destinationTriangle1 < 24 && destinationTriangle1 > -1)
                    {
                        if (Board.Triangles[destinationTriangle1].CheckersCount == 1 &&
                            Board.Triangles[destinationTriangle1].Type == CheckerType.White)
                        {
                            canMove = true;
                        }
                        if (Board.Triangles[destinationTriangle1].Type == CheckerType.None)
                        {
                            canMove = true;
                        }
                        if (Board.Triangles[destinationTriangle1].Type == CheckerType.Black)
                        {
                            canMove = true;
                        }
                    }
                    if (destinationTriangle2 < 24 && destinationTriangle2 > -1)
                    {
                        if (Board.Triangles[destinationTriangle2].CheckersCount == 1 &&
                            Board.Triangles[destinationTriangle2].Type == CheckerType.White)
                        {
                            canMove = true;
                        }
                        if (Board.Triangles[destinationTriangle2].Type == CheckerType.None)
                        {
                            canMove = true;
                        }
                        if (Board.Triangles[destinationTriangle2].Type == CheckerType.Black)
                        {
                            canMove = true;
                        }
                    }
                });
            }
            return canMove;
        }

        private bool ThereAreAnyavailableMovesFromDeadBar()
        {
            var canMove = false;
            if (DeadCheckersBar.Bar.Count > 0)
            {
                var destinationTriangle1 = int.MinValue;
                var destinationTriangle2 = int.MinValue;
                if (Dice.FirstCube != 0)
                {
                    destinationTriangle1 = 24 - Dice.FirstCube;
                }
                if (Dice.SecondCube != 0)
                {
                    destinationTriangle2 = 24 - Dice.SecondCube;
                }
                if (destinationTriangle1 < 24 && destinationTriangle1 > -1)
                {
                    if (Board.Triangles[destinationTriangle1].CheckersCount == 1 &&
                        Board.Triangles[destinationTriangle1].Type == CheckerType.White)
                    {
                        canMove = true;
                    }
                    if (Board.Triangles[destinationTriangle1].Type == CheckerType.None)
                    {
                        canMove = true;
                    }
                    if (Board.Triangles[destinationTriangle1].Type == CheckerType.Black)
                    {
                        canMove = true;
                    }
                }
                if (destinationTriangle2 < 24 && destinationTriangle2 > -1)
                {
                    if (Board.Triangles[destinationTriangle2].CheckersCount == 1 &&
                        Board.Triangles[destinationTriangle2].Type == CheckerType.White)
                    {
                        canMove = true;
                    }
                    if (Board.Triangles[destinationTriangle2].Type == CheckerType.None)
                    {
                        canMove = true;
                    }
                    if (Board.Triangles[destinationTriangle2].Type == CheckerType.Black)
                    {
                        canMove = true;
                    }
                }
            }
            return canMove;
        }

        private void UpdateDiceAfterMoveFromDeadBar(int move)
        {
            if (Dice.RolledDouble)
            {
                Dice.DecrementSteps();
                if (Dice.Steps == 2)
                {
                    Dice.ResetFirstCube();
                }
                if (Dice.Steps == 0)
                {
                    Dice.ResetSecondCube();
                }
            }
            else
            {
                if (move == 24 - Dice.FirstCube + 1)
                {
                    Dice.ResetFirstCube();
                }
                else if (move == 24 - Dice.SecondCube + 1)
                {
                    Dice.ResetSecondCube();
                }
                Dice.DecrementSteps();
            }
        }

        private void UpdateDice(int move)
        {
            if (Dice.RolledDouble)
            {
                Dice.DecrementSteps();
                if (Dice.Steps == 2)
                {
                    Dice.ResetFirstCube();
                }
                if (Dice.Steps == 0)
                {
                    Dice.ResetSecondCube();
                }
            }
            else
            {
                if (move == Dice.FirstCube)
                {
                    Dice.ResetFirstCube();
                }
                else if (move == Dice.SecondCube)
                {
                    Dice.ResetSecondCube();
                }
                Dice.DecrementSteps();
            }
        }

        private bool AllCheckersInLocalArea(int start, int end)
        {
            if (DeadCheckersBar.Bar.Count > 0)
            {
                return false;
            }
            var allCheckersInLocalArea = 1;
            Parallel.For(start, end, (i) =>
            {
                if (Board.Triangles[i].Type == CheckerType.Black)
                {
                    Interlocked.Exchange(ref allCheckersInLocalArea, 0);
                }
            });
            return allCheckersInLocalArea == 1;
        }

    }
}
