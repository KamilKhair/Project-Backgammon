namespace BackgammonLib
{
    internal class BlackPlayer : Player
    {
        public BlackPlayer(CheckerType t, IBackgammon game) : base(t, game)
        {
            DeadCheckersBar = new BlackDeadCheckersBar();
            OutSideCheckersBar = new BlackOutSideCheckersBar();
        }

        public override bool Move(int triangle, int move)
        {
            if (!CanPerformMove(triangle, move))
            {
                return false;
            }
            PerformTheCorrectMove(triangle, move);
            UpdateDice(move);
            CheckIfNoMoreAvailableMoves();
            return true;
        }

        private void CheckIfNoMoreAvailableMoves()
        {
            if (CheckIfThereAreAvailableMoves())
            {
                return;
            }
            var backgammonGame = Game as Backgammon;
            if (Dice.Steps > 0)
            {
                backgammonGame?.RaiseNoAvailableMovesEvent(Dice.FirstCube, Dice.SecondCube);
            }
            if (backgammonGame != null)
            {
                backgammonGame.Turn = CheckerType.White;
            }
            (Dice as Dice)?.ResetDice();
        }

        private void PerformTheCorrectMove(int triangle, int move)
        {
            if (triangle <= move)
            {
                MoveToOutsideBar(triangle);
            }
            else
            {
                PerformMove(triangle, move);
            }
        }

        private bool CanPerformMove(int triangle, int move)
        {
            return CheckIfThereAreAvailableMoves() && CheckIfCanMove(triangle, move);
        }

        private void PerformMove(int triangle, int move)
        {
            var destinationTriangle = Board.Triangles[triangle - 1 - move] as Triangle;
            var sourceTriangle = Board.Triangles[triangle - 1] as Triangle;
            var sourceChecker = sourceTriangle?.CheckersStack.Pop() as Checker;

            //Option 1: Moving to a triangle which has only one white checker.
            if (destinationTriangle != null && (destinationTriangle.Type == CheckerType.White && destinationTriangle.CheckersCount == 1))
            {
                KillTheWhiteChecker(destinationTriangle, sourceChecker);
            }
            //Option 2: Moving to an empty triangle or a triangle which has at least one black checker.
            else if (destinationTriangle != null && (destinationTriangle.Type == CheckerType.Black || destinationTriangle.Type == CheckerType.None))
            {
                MoveToBlackOrEmptyTriangle(destinationTriangle, sourceChecker);
            }
            if (sourceChecker != null)
            {
                sourceChecker.CheckerTriangle -= move;
            }
            if (sourceTriangle == null)
            {
                return;
            }
            --sourceTriangle.CheckersCount;
            TryResetSourceTriangle(sourceTriangle);
        }

        private static void TryResetSourceTriangle(Triangle sourceTriangle)
        {
            if (sourceTriangle.CheckersCount != 0)
            {
                return;
            }
            sourceTriangle.Type = CheckerType.None;
            sourceTriangle.IsEmpty = true;
        }

        private static void MoveToBlackOrEmptyTriangle(Triangle destinationTriangle, IChecker sourceChecker)
        {
            ++destinationTriangle.CheckersCount;
            destinationTriangle.CheckersStack.Push(sourceChecker);
            if (!destinationTriangle.IsEmpty)
            {
                return;
            }
            destinationTriangle.IsEmpty = false;
            destinationTriangle.Type = CheckerType.Black;
        }

        private void KillTheWhiteChecker(Triangle destinationTriangle, IChecker sourceChecker)
        {
            var destinationChecker = destinationTriangle.CheckersStack.Pop() as Checker;
            if (destinationChecker != null)
            {
                destinationChecker.CheckerTriangle = -1; // -1 = Dead;
                destinationChecker.IsAlive = false;
                (Game as Backgammon)?.WhitePlayer.DeadCheckersBar.AddToBar(destinationChecker);
            }
            destinationTriangle.CheckersStack.Push(sourceChecker);
            destinationTriangle.Type = CheckerType.Black;
        }

        public override bool MoveFromDeadBar(int triangle)
        {
            if (!CheckIfThereAreAvailableMoves())
            {
                return false;
            }
            if (!CheckIfCanMoveFromDeadBar(triangle))
            {
                return false;
            }
            var destinationTriangle = Board.Triangles[triangle - 1] as Triangle;
            var sourceChecker = DeadCheckersBar.RemoveFromBar();
            sourceChecker.IsAlive = true;
            sourceChecker.CheckerTriangle = triangle - 1;

            //Option 1: Moving to a triangle which has only one white checker.
            if (destinationTriangle != null && (destinationTriangle.CheckersCount == 1 && destinationTriangle.Type == CheckerType.White))
            {
                KillTheWhiteChecker(destinationTriangle, sourceChecker);
                UpdateDiceAfterMoveFromDeadBar(triangle);
                CheckIfNoMoreAvailableMoves();
                return true;
            }

            //Option 2: Moving to an empty triangle or a triangle which has at least one black checker.
            MoveToBlackOrEmptyTriangle(destinationTriangle, sourceChecker);
            UpdateDiceAfterMoveFromDeadBar(triangle);
            CheckIfNoMoreAvailableMoves();
            return true;
        }

        private void MoveToOutsideBar(int triangle)
        {
            var sourceTriangle = Board.Triangles[triangle - 1] as Triangle;
            var sourceChecker = sourceTriangle?.CheckersStack.Pop() as Checker;
            if (sourceChecker != null)
            {
                sourceChecker.CheckerTriangle = -2; // -2 = outside
                sourceChecker.IsFinished = true;
                sourceChecker.IsAlive = false;
                OutSideCheckersBar.AddToBar(Game, sourceChecker);
            }
            if (sourceTriangle == null)
            {
                return;
            }
            --sourceTriangle.CheckersCount;
            if (sourceTriangle.CheckersCount != 0) return;
            sourceTriangle.Type = CheckerType.None;
            sourceTriangle.IsEmpty = true;
        }

        public override bool Roll()
        {
            var rollDice = (Dice as Dice)?.RollDice(CheckerType.Black);
            return rollDice != null && (Dice.Steps == 0 && (bool) rollDice);
        }

        public override bool CheckIfCanMove(int triangle, int move)
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
            if (DeadCheckersBar.Bar.Count > 0)
            {
                return false;
            }
            if (AllCheckersInLocalArea(6, 24))
            {
                if (triangle - move < 0)
                {
                    return AllCheckersInLocalArea(triangle, 24);
                }
                if (triangle == move)
                {
                    return true;
                }         
            }
            var destinationTriangLe = triangle - move - 1;
            if (destinationTriangLe < 0 || destinationTriangLe > 23)
            {
                return false;
            }
            return Board.Triangles[destinationTriangLe].Type != CheckerType.White || Board.Triangles[destinationTriangLe].CheckersCount <= 1;
        }

        public override bool CheckIfCanMoveFromDeadBar(int triangle)
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
            return Board.Triangles[destinationTriangLe].Type != CheckerType.White || Board.Triangles[destinationTriangLe].CheckersCount <= 1;
        }


        public override bool CheckIfThereAreAvailableMoves()
        {
            if (ThereAreAnyavailableMoves())
            {
                return true;
            }
            return ThereAreAnyavailableMovesFromDeadBar() || ThereAreAnyAvailableMovesToOutSide();
        }

        private bool ThereAreAnyAvailableMovesToOutSide()
        {
            if (DeadCheckersBar.Bar.Count > 0 || !AllCheckersInLocalArea(6, 24))
            {
                return false;
            }
            if (Dice.FirstCube != 0 &&
                (Board.Triangles[Dice.FirstCube - 1].Type != CheckerType.White ||
                 Board.Triangles[Dice.FirstCube - 1].CheckersCount <= 1))
            {
                return true;
            }
            return Dice.SecondCube != 0 && (Board.Triangles[Dice.SecondCube - 1].Type != CheckerType.White || Board.Triangles[Dice.SecondCube - 1].CheckersCount <= 1);
        }

        private bool ThereAreAnyavailableMoves()
        {
            var canMove = false;
            if (!AllCheckersInLocalArea(6, 24) && DeadCheckersBar.Bar.Count == 0)
            {
                canMove = AnyAvailableMove();
            }
            return canMove;
        }

        private bool AnyAvailableMove()
        {
            var canMove = false;
            for (var i = 0; i < 24; ++i)
            {
                if (Board.Triangles[i].Type != CheckerType.Black)
                {
                    continue;
                }
                var destinationTriangle1 = int.MinValue;
                var destinationTriangle2 = int.MinValue;
                FindDestinationTriangles(ref destinationTriangle1, i, ref destinationTriangle2);
                CheckFirstDestination(ref canMove, destinationTriangle1);
                CheckSecondDestination(destinationTriangle2, ref canMove);
            }
            return canMove;
        }

        private void CheckSecondDestination(int destinationTriangle2, ref bool canMove)
        {
            if (destinationTriangle2 >= 24 || destinationTriangle2 <= -1)
            {
                return;
            }
            MovingToWhiteTriangleWithOnlyOneChecker(destinationTriangle2, ref canMove);
            MovingToEmptyTriangle(destinationTriangle2, ref canMove);
            MovingToBlackTriangle(destinationTriangle2, ref canMove);
        }

        private void MovingToBlackTriangle(int destinationTriangle2, ref bool canMove)
        {
            if (Board.Triangles[destinationTriangle2].Type == CheckerType.Black)
            {
                canMove = true;
            }
        }

        private void MovingToEmptyTriangle(int destinationTriangle2, ref bool canMove)
        {
            if (Board.Triangles[destinationTriangle2].Type == CheckerType.None)
            {
                canMove = true;
            }
        }

        private void MovingToWhiteTriangleWithOnlyOneChecker(int destinationTriangle2, ref bool canMove)
        {
            if (Board.Triangles[destinationTriangle2].CheckersCount == 1 &&
                Board.Triangles[destinationTriangle2].Type == CheckerType.White)
            {
                canMove = true;
            }
        }

        private void CheckFirstDestination(ref bool canMove, int destinationTriangle1)
        {
            if (destinationTriangle1 >= 24 || destinationTriangle1 <= -1)
            {
                return;
            }
            MovingToWhiteTriangleWithOnlyOneChecker(destinationTriangle1, ref canMove);
            MovingToEmptyTriangle(destinationTriangle1, ref canMove);
            MovingToBlackTriangle(destinationTriangle1, ref canMove);
        }

        private void FindDestinationTriangles(ref int destinationTriangle1, int i, ref int destinationTriangle2)
        {
            if (Dice.FirstCube != 0)
            {
                destinationTriangle1 = i - Dice.FirstCube;
            }
            if (Dice.SecondCube != 0)
            {
                destinationTriangle2 = i - Dice.SecondCube;
            }
        }

        private bool ThereAreAnyavailableMovesFromDeadBar()
        {
            var canMove = false;
            if (DeadCheckersBar.Bar.Count <= 0)
            {
                return false;
            }
            var destinationTriangle1 = int.MinValue;
            var destinationTriangle2 = int.MinValue;
            FindDestinationTriangles(ref destinationTriangle1, 24, ref destinationTriangle2);
            CheckFirstDestination(ref canMove, destinationTriangle1);
            CheckSecondDestination(destinationTriangle2, ref canMove);
            return canMove;
        }

        private void UpdateDiceAfterMoveFromDeadBar(int move)
        {
            if (Dice.RolledDouble)
            {
                UpdateDiceWhenRolledDouble();
            }
            else
            {
                if (move == 24 - Dice.FirstCube + 1)
                {
                    (Dice as Dice)?.ResetFirstCube();
                }
                else if (move == 24 - Dice.SecondCube + 1)
                {
                    (Dice as Dice)?.ResetSecondCube();
                }
                (Dice as Dice)?.DecrementSteps();
            }
        }

        private void UpdateDice(int move)
        {
            if (Dice.RolledDouble)
            {
                UpdateDiceWhenRolledDouble();
            }
            else
            {
                if (move == Dice.FirstCube)
                {
                    (Dice as Dice)?.ResetFirstCube();
                }
                else if (move == Dice.SecondCube)
                {
                    (Dice as Dice)?.ResetSecondCube();
                }
                (Dice as Dice)?.DecrementSteps();
            }
        }

        private void UpdateDiceWhenRolledDouble()
        {
            (Dice as Dice)?.DecrementSteps();
            if (Dice.Steps == 2)
            {
                (Dice as Dice)?.ResetFirstCube();
            }
            if (Dice.Steps == 0)
            {
                (Dice as Dice)?.ResetSecondCube();
            }
        }

        private bool AllCheckersInLocalArea(int start, int end)
        {
            return DeadCheckersBar.Bar.Count <= 0 && CheckIfAllCheckersInLocalArea(start, end);
        }

        private bool CheckIfAllCheckersInLocalArea(int start, int end)
        {
            var allCheckersInLocalArea = true;
            for (var i = start; i < end; ++i)
            {
                if (Board.Triangles[i].Type != CheckerType.Black)
                {
                    continue;
                }
                allCheckersInLocalArea = false;
                break;
            }
            return allCheckersInLocalArea;
        }

        public override bool IsBlackPlayerCanPlay => CheckIfPlayerCanPlay();

        private bool CheckIfPlayerCanPlay()
        {
            var notReachableTriangles = 0;
            for (var i = 18; i < 24; ++i)
            {
                if (Board.Triangles[i].Type == CheckerType.White && Board.Triangles[i].CheckersCount > 1)
                {
                    ++notReachableTriangles;
                }
                else
                {
                    break;
                }
            }
            return notReachableTriangles != 6;
        }
    }
}
