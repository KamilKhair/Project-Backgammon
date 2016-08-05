
using System.Threading;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class WhitePlayer : Player
    {
        
        public WhitePlayer(string name, CheckerType t) : base(name, t)
        {
            _deadCheckersBar = new WhiteDeadChechersBar();
            _outSideCheckersBar = new WhiteOutSideCheckersBar();
        }

        internal override bool Roll(Backgammon game)
        {
            return game.Dice.Steps == 0 && game.Dice.RollDice(game, CheckerType.White);
        }

        internal override bool Move(Backgammon game, int triangle, int move)
        {
            if (triangle + move >= 25)
            {
                if (MoveToOutsideBar(game, triangle, move))
                {
                    return true;
                }
            }
            if (!CanMove(game, triangle, move))
            {
                return false;
            }

            PerformMove(game, triangle, move);

            UpdateTurn(game);
            return true;
        }

        private bool CanMove(Backgammon game, int triangle, int move)
        {
            if (move > 6 || move < 1)
            {
                return false;
            }
            if (triangle < 1 || triangle > 24)
            {
                return false;
            }
            if (game._whitePlayer._deadCheckersBar.Bar.Count > 0)
            {
                return false;
            }

            if (game._dice.Steps == 0)
            {
                return false;
            }

            var destination = triangle -1 + move;
            if (destination > 23)
            {
                return false;
            }

            var destinationTriangle = game._board.Triangles[destination];
            if (destinationTriangle.Type == CheckerType.Black && destinationTriangle.CheckersCount > 1)
            {
                return false;
            }

            var sourceTriangle = game._board.Triangles[triangle - 1];
            if (sourceTriangle.IsEmpty || sourceTriangle.Type == CheckerType.Black)
            {
                return false;
            }

            return UpdateDice(game, move);
        }

        private bool UpdateDice(Backgammon game, int move, bool fromDeadBar = false)
        {
            if (game._dice.RolledDouble)
            {
                if (fromDeadBar)
                {
                    if (move != game._dice.FirstCube && move != game._dice.SecondCube)
                    {
                        return false;
                    }
                }
                else
                {
                    if (move != game._dice.FirstCube && move != game._dice.SecondCube)
                    {
                        return false;
                    }
                }
                game._dice.DecrementSteps(game);
                if (game._dice.Steps == 2)
                {
                    game._dice.ResetFirstCube();
                }
                if (game._dice.Steps == 0)
                {
                    game._dice.ResetSecondCube();
                }
            }
            else
            {
                if (fromDeadBar)
                {
                    if (move == game._dice.FirstCube)
                    {
                        game._dice.ResetFirstCube();
                    }
                    else if (move == game._dice.SecondCube)
                    {
                        game._dice.ResetSecondCube();
                    }
                    else
                    {
                        return false;
                    }
                    game._dice.DecrementSteps(game);
                }
                else
                {
                    if (move == game._dice.FirstCube)
                    {
                        game._dice.ResetFirstCube();
                    }
                    else if (move == game._dice.SecondCube)
                    {
                        game._dice.ResetSecondCube();
                    }
                    else
                    {
                        return false;
                    }
                    game._dice.DecrementSteps(game);
                }
            }
            if (fromDeadBar && _deadCheckersBar.Bar.Count > 2)
            {
                if (move != game._dice.FirstCube && move != game._dice.SecondCube)
                {
                    game.Turn = CheckerType.Black;
                    game._dice.ResetDice();
                }
            }
            return true;
        }


        private void PerformMove(Backgammon game, int triangle, int move)
        {
            var destinationTriangle = game._board.Triangles[triangle - 1 + move];
            var sourceTriangle = game._board.Triangles[triangle - 1];
            var sourceChecker = sourceTriangle.CheckersStack.Pop();

            //Option 1: Moving to a triangle which has only one Black checker.
            if (destinationTriangle.Type == CheckerType.Black && destinationTriangle.CheckersCount == 1)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead
                destinationChecker.IsAlive = false;
                game._blackPlayer._deadCheckersBar.AddToBar(destinationChecker);
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


        internal bool MoveToOutsideBar(Backgammon game, int triangle, int move)
        {
            if (!CanMoveToOutsideBar(game, triangle, move))
            {
                return false;
            }
            var sourceTriangle = game._board.Triangles[triangle - 1];
            var sourceChecker = sourceTriangle.CheckersStack.Pop();
            sourceChecker.CheckerTriangle = -2; // -2 = outside
            sourceChecker.IsFinished = true;
            sourceChecker.IsAlive = false;
            _outSideCheckersBar.AddToBar(game, sourceChecker);
            --sourceTriangle.CheckersCount;
            if (sourceTriangle.CheckersCount != 0) return true;
            sourceTriangle.Type = CheckerType.None;
            sourceTriangle.IsEmpty = true;

            return true;
        }

        private bool CanMoveToOutsideBar(Backgammon game, int triangle, int move)
        {
            if (move > 6 || move < 1)
            {
                return false;
            }
            if (triangle < 19 || triangle > 24)
            {
                return false;
            }
            if (game._board.Triangles[triangle - 1].Type != CheckerType.White)
            {
                return false;
            }
            if (triangle + move > 25)
            {
                if (!AllCheckersInLocalArea(game, 0, triangle - 1))
                {
                    return false;
                }
            }
            else
            {
                if (!AllCheckersInLocalArea(game, 0, 18))
                {
                    return false;
                }
            }

            UpdateDice(game, move);
            UpdateTurn(game);
            return true;
        }

        private bool AllCheckersInLocalArea(Backgammon game, int start, int end)
        {
            if (_deadCheckersBar.Bar.Count > 0)
            {
                return false;
            }
            var allCheckersInLocalArea = 1;
            Parallel.For(start, end, (i) =>
            {
                if (game._board.Triangles[i].Type == CheckerType.White)
                {
                    Interlocked.Exchange(ref allCheckersInLocalArea, 0);
                }
            });
            return allCheckersInLocalArea == 1;
        }


        internal override bool MoveFromDeadBar(Backgammon game, int move)
        {
            if (!CanMoveFromDeadBar(game, move))
            {
                return false;
            }

            var destinationTriangle = game._board.Triangles[move - 1];
            var sourceChecker = _deadCheckersBar.RemoveFromBar();
            sourceChecker.IsAlive = true;
            sourceChecker.CheckerTriangle = move - 1;

            //Option 1: Moving to a triangle which has only one Black checker.
            if (destinationTriangle.CheckersCount == 1 && destinationTriangle.Type == CheckerType.Black)
            {
                var destinationChecker = destinationTriangle.CheckersStack.Pop();
                destinationChecker.CheckerTriangle = -1; // -1 = Dead
                destinationChecker.IsAlive = false;
                game._blackPlayer._deadCheckersBar.Bar.Push(destinationChecker);
                destinationTriangle.CheckersStack.Push(sourceChecker);
                destinationTriangle.Type = CheckerType.White;
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

            UpdateTurnIfPlayerCanNotPlay(game);
            return true;
        }

        private void UpdateTurnIfPlayerCanNotPlay(Backgammon game)
        {
            if (_deadCheckersBar.Bar.Count > 0)
            {
                if (game._dice.FirstCube != 0 && game._board.Triangles[game._dice.FirstCube - 1].Type == CheckerType.Black)
                {
                    if (game._dice.FirstCube != 0 && game._board.Triangles[game._dice.FirstCube - 1].CheckersCount > 1)
                    {
                        if (game._dice.SecondCube != 0 && game._board.Triangles[game._dice.SecondCube - 1].Type == CheckerType.Black)
                        {
                            if (game._dice.SecondCube != 0 && game._board.Triangles[game._dice.SecondCube - 1].CheckersCount > 1)
                            {
                                game.Turn = CheckerType.Black;
                                game._dice.ResetDice();
                            }
                        }
                    }
                }
            }
            else
            {
                UpdateTurn(game);
            }
        }

        private bool CanMoveFromDeadBar(Backgammon game, int move)
        {
            if (game._whitePlayer._deadCheckersBar.Bar.Count == 0)
            {
                return false;
            }
            if (game.Turn != CheckerType.White)
            {
                return false;
            }
            if (!game.IsWhitePlayerCanPlay)
            {
                return false;
            }
            if (move > 6 || move < 1)
            {
                return false;
            }

            var destinationTriangle = game._board.Triangles[move - 1];

            if (destinationTriangle.CheckersCount > 1 && destinationTriangle.Type == CheckerType.Black)
            {
                return false;
            }

            return UpdateDice(game, move, true);
        }

        private void UpdateTurn(Backgammon game)
        {
            if (AllCheckersInLocalArea(game, 0, 18))
            {
                return;
            }
            var canMove = false;
            var firstCube = game._dice.FirstCube;
            var secondCube = game._dice.SecondCube;
            Parallel.For(0, 24, (i) =>
            {
                if (i + firstCube > 23 || i + secondCube > 23)
                {
                }
                else if (game._board.Triangles[i + firstCube].Type == CheckerType.Black && 
                         game._board.Triangles[i + firstCube].CheckersCount == 1 ||
                         game._board.Triangles[i + firstCube].Type == CheckerType.White ||
                         game._board.Triangles[i + firstCube].Type == CheckerType.None)
                {
                    if (firstCube != 0)
                    {
                        canMove = true;
                    }
                }
                else if (game._board.Triangles[i + secondCube].Type == CheckerType.Black &&
                         game._board.Triangles[i + secondCube].CheckersCount == 1 ||
                         game._board.Triangles[i + secondCube].Type == CheckerType.White ||
                         game._board.Triangles[i + secondCube].Type == CheckerType.None)
                {
                    if (secondCube != 0)
                    {
                        canMove = true;
                    }
                }
            });

            if (!canMove)
            {
                game.Turn = CheckerType.Black;
                game._dice.ResetDice();
            }
        }
    }
}
