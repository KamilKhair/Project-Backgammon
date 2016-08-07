using System.Threading;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class BlackPlayer : Player
    {
        internal BlackPlayer(CheckerType t, GameBoard board, Dice dice, Backgammon game) : base(t, board, dice, game)
        {
            DeadCheckersBar = new BlackDeadCheckersBar();
            OutSideCheckersBar = new BlackOutSideCheckersBar();
        }

        internal override bool Roll()
        {
            return Dice.Steps == 0 && Dice.RollDice(CheckerType.Black);
        }

        internal override bool Move(int triangle, int move)
        {
            if (move != Dice.FirstCube && move != Dice.SecondCube)
            {
                return false;
            }
            if (triangle <= move)
            {
                if (MoveToOutsideBar(triangle, move))
                {
                    return true;
                }
            }
            if (!CanMove(triangle, move))
            {
                return false;
            }
            
            PerformMove(triangle, move);
            UpdateTurn();

            return true;
        }

        private bool CanMove(int triangle, int move)
        {
            if (move > 6 || move < 1)
            {
                return false;
            }
            if (triangle < 1 || triangle > 24)
            {
                return false;
            }
            if (DeadCheckersBar.Bar.Count > 0)
            {
                return false;
            }

            if (Dice.Steps == 0)
            {
                return false;
            }

            var destination = triangle - 1 - move;
            if (destination < 0)
            {
                return false;
            }

            var destinationTriangle = Board.Triangles[destination];
            if (destinationTriangle.Type == CheckerType.White && destinationTriangle.CheckersCount > 1)
            {
                return false;
            }

            var sourceTriangle = Board.Triangles[triangle - 1];
            if (sourceTriangle.IsEmpty || sourceTriangle.Type == CheckerType.White)
            {
                return false;
            }

            return UpdateDice(move);
        }

        private bool UpdateDice(int move, bool fromDeadBar = false)
        {
            if (Dice.RolledDouble)
            {
                if (fromDeadBar)
                {
                    if (move != 24 - Dice.FirstCube + 1 && move != 24 - Dice.SecondCube + 1)
                    {
                        return false;
                    }
                }
                else
                {
                    if (move != Dice.FirstCube && move != Dice.SecondCube)
                    {
                        return false;
                    }
                }
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
                if (fromDeadBar)
                {
                    if (move == 24 - Dice.FirstCube + 1)
                    {
                        Dice.ResetFirstCube();
                    }
                    else if (move == 24 - Dice.SecondCube + 1)
                    {
                        Dice.ResetSecondCube();
                    }
                    else
                    {
                        return false;
                    }
                    Dice.DecrementSteps();
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
                    else
                    {
                        return false;
                    }
                    Dice.DecrementSteps();
                }
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


        private bool MoveToOutsideBar(int triangle, int move)
        {
            if (!CanMoveToOutsideBar(triangle, move))
            {
                return false;
            }
            var sourceTriangle = Board.Triangles[triangle - 1];
            var sourceChecker = sourceTriangle.CheckersStack.Pop();
            sourceChecker.CheckerTriangle = -2; // -2 = outside
            sourceChecker.IsFinished = true;
            sourceChecker.IsAlive = false;
            OutSideCheckersBar.AddToBar(Game, sourceChecker);
            --sourceTriangle.CheckersCount;
            if (sourceTriangle.CheckersCount != 0) return true;
            sourceTriangle.Type = CheckerType.None;
            sourceTriangle.IsEmpty = true;
            
            return true;
        }

        private bool CanMoveToOutsideBar(int triangle, int move)
        {
            if (move > 6 || move < 1)
            {
                return false;
            }
            if (triangle < 1 || triangle > 6)
            {
                return false;
            }
            if (Board.Triangles[triangle - 1].Type != CheckerType.Black)
            {
                return false;
            }
            if (triangle < move)
            {
                if (!AllCheckersInLocalArea(triangle, 24 - triangle))
                {
                    return false;
                }
            }
            else
            {
                if (!AllCheckersInLocalArea(6, 24))
                {
                    return false;
                }
            }

            UpdateTurn();

            UpdateDice(move);
            return true;
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


        internal override bool MoveFromDeadBar(int move)
        {
            if (!CanMoveFromDeadBar(move))
            {
                return false;
            }

            var destinationTriangle = Board.Triangles[move - 1];
            var sourceChecker = DeadCheckersBar.RemoveFromBar();
            sourceChecker.IsAlive = true;
            sourceChecker.CheckerTriangle = move - 1;

            //Option 1: Moving to a triangle which has only one white checker.
            if (destinationTriangle.CheckersCount == 1 && destinationTriangle.Type == CheckerType.White)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead
                destinationChecker.IsAlive = false;
                Game.WhitePlayer.DeadCheckersBar.Bar.Push(destinationChecker);
                destinationTriangle.CheckersStack.Push(sourceChecker);
                destinationTriangle.Type = CheckerType.Black;
                UpdateTurnIfPlayerCanNotPlay();
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

            UpdateTurnIfPlayerCanNotPlay();
            return true;
        }

        private void UpdateTurnIfPlayerCanNotPlay()
        {
            if (DeadCheckersBar.Bar.Count > 0)
            {
                if (Dice.FirstCube != 0 && Board.Triangles[24 - Dice.FirstCube].Type == CheckerType.White)
                {
                    if (Dice.FirstCube != 0 && Board.Triangles[24 - Dice.FirstCube].CheckersCount > 1)
                    {
                        if (Dice.SecondCube != 0 && Board.Triangles[24 - Dice.SecondCube].Type == CheckerType.White)
                        {
                            if (Dice.SecondCube != 0 && Board.Triangles[24 - Dice.SecondCube].CheckersCount > 1)
                            {
                                Game.Turn = CheckerType.White;
                                Dice.ResetDice();
                            }
                        }
                    }
                }
            }
            else
            {
                UpdateTurn();
            }
            if (DeadCheckersBar.Bar.Count > 0 && Dice.Steps > 0)
            {
                var canNotPlay = 0;
                switch (Dice.FirstCube)
                {
                    case 0:
                        break;
                    default:
                        if (Board.Triangles[24 - Dice.FirstCube].Type == CheckerType.White)
                        {
                            ++canNotPlay;
                        }
                        break;
                }
                switch (Dice.SecondCube)
                {
                    case 0:
                        break;
                    default:
                        if (Board.Triangles[24 - Dice.SecondCube].Type == CheckerType.White)
                        {
                            ++canNotPlay;
                        }
                        break;
                }
                if (canNotPlay == 1 && (Dice.FirstCube == 0 || Dice.SecondCube == 0) || canNotPlay == 2)
                {
                    Game.Turn = CheckerType.White;
                    Dice.ResetDice();
                }
            }
        }

        private bool CanMoveFromDeadBar(int move)
        {
            if (DeadCheckersBar.Bar.Count == 0)
            {
                return false;
            }
            if (Game.Turn != CheckerType.Black)
            {
                return false;
            }
            if (!Game.IsBlackPlayerCanPlay)
            {
                return false;
            }
            if (move > 24 || move < 19)
            {
                return false;
            }

            var destinationTriangle = Board.Triangles[move - 1];

            if (destinationTriangle.CheckersCount > 1 && destinationTriangle.Type == CheckerType.White)
            {
                return false;
            }


            return UpdateDice(move, true);
        }

        private void UpdateTurn()
        {
            if (AllCheckersInLocalArea(6, 24))
            {
                return;
            }
            var canMove = false;
            var firstCube = Dice.FirstCube;
            var secondCube = Dice.SecondCube;
            Parallel.For(0, 24, (i) =>
            {
                if (i - firstCube < 0 || i - secondCube < 0)
                {
                }
                else if (Board.Triangles[i - firstCube].Type == CheckerType.White &&
                         Board.Triangles[i - firstCube].CheckersCount == 1 ||
                         Board.Triangles[i - firstCube].Type == CheckerType.Black ||
                         Board.Triangles[i - firstCube].Type == CheckerType.None)
                {
                    if (firstCube != 0)
                    {
                        canMove = true;
                    }
                }
                else if (Board.Triangles[i - secondCube].Type == CheckerType.White &&
                         Board.Triangles[i - secondCube].CheckersCount == 1 ||
                         Board.Triangles[i - secondCube].Type == CheckerType.Black ||
                         Board.Triangles[i - secondCube].Type == CheckerType.None)
                {
                    if (secondCube != 0)
                    {
                        canMove = true;
                    }
                }
            });

            if (!canMove)
            {
                Game.Turn = CheckerType.White;
                Dice.ResetDice();
            }
        }
    }
}
