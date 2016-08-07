
using System.Threading;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class WhitePlayer : Player
    {
        
        public WhitePlayer(CheckerType t, GameBoard board, Dice dice, Backgammon game) : base(t, board, dice, game)
        {
            DeadCheckersBar = new WhiteDeadChechersBar();
            OutSideCheckersBar = new WhiteOutSideCheckersBar();
        }

        internal override bool Roll()
        {
            return Dice.Steps == 0 && Dice.RollDice(CheckerType.White);
        }

        internal override bool Move(int triangle, int move)
        {
            if (move != Dice.FirstCube && move != Dice.SecondCube)
            {
                return false;
            }
            if (triangle + move >= 25)
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

            var destination = triangle -1 + move;
            if (destination > 23)
            {
                return false;
            }

            var destinationTriangle = Board.Triangles[destination];
            if (destinationTriangle.Type == CheckerType.Black && destinationTriangle.CheckersCount > 1)
            {
                return false;
            }

            var sourceTriangle = Board.Triangles[triangle - 1];
            if (sourceTriangle.IsEmpty || sourceTriangle.Type == CheckerType.Black)
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
                    if (move != Dice.FirstCube && move != Dice.SecondCube)
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
            var destinationTriangle = Board.Triangles[triangle - 1 + move];
            var sourceTriangle = Board.Triangles[triangle - 1];
            var sourceChecker = sourceTriangle.CheckersStack.Pop();

            //Option 1: Moving to a triangle which has only one Black checker.
            if (destinationTriangle.Type == CheckerType.Black && destinationTriangle.CheckersCount == 1)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead
                destinationChecker.IsAlive = false;
                Game.BlackPlayer.DeadCheckersBar.AddToBar(destinationChecker);
                destinationTriangle.CheckersStack.Push(sourceChecker);
                destinationTriangle.Type = CheckerType.White;
            }
            //Option 2: Moving to an empty triangle or a triangle which has at least one White checker.
            else if (destinationTriangle.Type == CheckerType.White || destinationTriangle.Type == CheckerType.None)
            {
                ++destinationTriangle.CheckersCount;
                destinationTriangle.CheckersStack.Push(sourceChecker);
                if (destinationTriangle.IsEmpty)
                {
                    destinationTriangle.IsEmpty = false;
                    destinationTriangle.Type = CheckerType.White;
                }
            }
            sourceChecker.CheckerTriangle += move;
            --sourceTriangle.CheckersCount;
            if (sourceTriangle.CheckersCount == 0)
            {
                sourceTriangle.Type = CheckerType.None;
                sourceTriangle.IsEmpty = true;
            }
        }


        internal bool MoveToOutsideBar(int triangle, int move)
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
            if (triangle < 19 || triangle > 24)
            {
                return false;
            }
            if (Board.Triangles[triangle - 1].Type != CheckerType.White)
            {
                return false;
            }
            if (triangle + move > 25)
            {
                if (!AllCheckersInLocalArea(0, triangle - 1))
                {
                    return false;
                }
            }
            else
            {
                if (!AllCheckersInLocalArea(0, 18))
                {
                    return false;
                }
            }

            UpdateDice(move);
            UpdateTurn();
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
                if (Board.Triangles[i].Type == CheckerType.White)
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

            //Option 1: Moving to a triangle which has only one Black checker.
            if (destinationTriangle.CheckersCount == 1 && destinationTriangle.Type == CheckerType.Black)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead
                destinationChecker.IsAlive = false;
                Game.BlackPlayer.DeadCheckersBar.Bar.Push(destinationChecker);
                destinationTriangle.CheckersStack.Push(sourceChecker);
                destinationTriangle.Type = CheckerType.White;
                UpdateTurnIfPlayerCanNotPlay();
                return true;
            }

            //Option 2: Moving to an empty triangle or a triangle which has at least one White checker.
            ++destinationTriangle.CheckersCount;
            destinationTriangle.CheckersStack.Push(sourceChecker);
            if (destinationTriangle.IsEmpty)
            {
                destinationTriangle.IsEmpty = false;
                destinationTriangle.Type = CheckerType.White;
            }

            UpdateTurnIfPlayerCanNotPlay();
            return true;
        }

        private void UpdateTurnIfPlayerCanNotPlay()
        {
            if (DeadCheckersBar.Bar.Count > 0)
            {
                if (Dice.FirstCube != 0 && Board.Triangles[Dice.FirstCube - 1].Type == CheckerType.Black)
                {
                    if (Dice.FirstCube != 0 && Board.Triangles[Dice.FirstCube - 1].CheckersCount > 1)
                    {
                        if (Dice.SecondCube != 0 && Board.Triangles[Dice.SecondCube - 1].Type == CheckerType.Black)
                        {
                            if (Dice.SecondCube != 0 && Board.Triangles[Dice.SecondCube - 1].CheckersCount > 1)
                            {
                                Game.Turn = CheckerType.Black;
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
                        if (Board.Triangles[Dice.FirstCube - 1].Type == CheckerType.Black)
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
                        if (Board.Triangles[Dice.SecondCube - 1].Type == CheckerType.Black)
                        {
                            ++canNotPlay;
                        }
                        break;
                }
                if (canNotPlay == 1 && (Dice.FirstCube == 0 || Dice.SecondCube == 0) || canNotPlay == 2)
                {
                    Game.Turn = CheckerType.Black;
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
            if (Game.Turn != CheckerType.White)
            {
                return false;
            }
            if (!Game.IsWhitePlayerCanPlay)
            {
                return false;
            }
            if (move > 6 || move < 1)
            {
                return false;
            }

            var destinationTriangle = Board.Triangles[move - 1];

            if (destinationTriangle.CheckersCount > 1 && destinationTriangle.Type == CheckerType.Black)
            {
                return false;
            }

            return UpdateDice(move, true);
        }

        private void UpdateTurn()
        {
            if (AllCheckersInLocalArea(0, 18))
            {
                return;
            }
            var canMove = false;
            var firstCube = Dice.FirstCube;
            var secondCube = Dice.SecondCube;
            Parallel.For(0, 24, (i) =>
            {
                if (i + firstCube > 23 || i + secondCube > 23)
                {
                }
                else if (Board.Triangles[i + firstCube].Type == CheckerType.Black && 
                         Board.Triangles[i + firstCube].CheckersCount == 1 ||
                         Board.Triangles[i + firstCube].Type == CheckerType.White ||
                         Board.Triangles[i + firstCube].Type == CheckerType.None)
                {
                    if (firstCube != 0)
                    {
                        canMove = true;
                    }
                }
                else if (Board.Triangles[i + secondCube].Type == CheckerType.Black &&
                         Board.Triangles[i + secondCube].CheckersCount == 1 ||
                         Board.Triangles[i + secondCube].Type == CheckerType.White ||
                         Board.Triangles[i + secondCube].Type == CheckerType.None)
                {
                    if (secondCube != 0)
                    {
                        canMove = true;
                    }
                }
            });

            if (!canMove)
            {
                Game.Turn = CheckerType.Black;
                Dice.ResetDice();
            }
        }
    }
}
