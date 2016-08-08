using System;
using System.Linq;
using BackgammonLib;

namespace BackgammonConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IBackgammon game = new Backgammon();
            while (!game.IsGameOver)
            {
                Console.Clear();
                DisplayOnConsole(game);
                Console.WriteLine($"Turn: {game.Turn} Player!");
                if (game.Dice.Steps == 0)
                {
                    Roll(game);
                }
                else
                {
                    int triangle;
                    var move = 0;
                    if (game.BlackDeadCheckersBar.Any() && game.Turn == CheckerType.Black)
                    {
                        triangle = GetFromUserTriangleToMoveInto(game);
                    }
                    else if (game.WhiteDeadCheckersBar.Any() && game.Turn == CheckerType.White)
                    {
                        triangle = GetFromUserTriangleToMoveInto(game);
                    }
                    else
                    {
                        triangle = GetFromUserTriangleToMoveFrom(game);
                        move = GetFromUserHowManyTrianglesToMove(game, triangle);
                    }
                    if (game.Turn == CheckerType.Black)
                    {
                        MoveBlack(game, triangle, move);
                    }
                    else
                    {
                        MoveWhite(game, triangle, move);
                    }
                }
            }
            var winner = game.Winner == CheckerType.Black ? "Black Player" : "White Player";
            Console.WriteLine($"The Winner is {winner}!");
        }

        private static void MoveWhite(IBackgammon game, int triangle, int move)
        {
            if (game.WhiteDeadCheckersBar.Any())
            {
                game.MoveWhiteFromDeadBar(triangle);
            }
            else
            {
                game.MoveWhite(triangle, move);
            }
        }

        private static void MoveBlack(IBackgammon game, int triangle, int move)
        {
            if (game.BlackDeadCheckersBar.Any())
            {
                game.MoveBlackFromDeadBar(triangle);
            }
            else
            {
                game.MoveBlack(triangle, move);
            }
        }

        private static int GetFromUserHowManyTrianglesToMove(IBackgammon game, int triangle)
        {
            int move;
            Console.WriteLine("Please Enter how many triangles to move (According to your Cubes!):");
            while (!int.TryParse(Console.ReadLine(), out move))
            {
                Console.Clear();
                DisplayOnConsole(game);
                Console.WriteLine($"Turn: {game.Turn} Player!");
                Console.WriteLine($"You rolled: ({game.Dice.FirstCube}, {game.Dice.SecondCube})");
                Console.WriteLine("Please Enter triangle to move from:");
                Console.WriteLine(triangle);
                Console.WriteLine("Please Enter how many triangles to move (According to your Cubes!):");
            }
            return move;
        }

        private static int GetFromUserTriangleToMoveFrom(IBackgammon game)
        {
            int triangle;
            Console.WriteLine($"You rolled: ({game.Dice.FirstCube}, {game.Dice.SecondCube})");
            Console.WriteLine("Please Enter triangle to move from:");
            while (!int.TryParse(Console.ReadLine(), out triangle))
            {
                Console.Clear();
                DisplayOnConsole(game);
                Console.WriteLine($"Turn: {game.Turn} Player!");
                Console.WriteLine($"You rolled: ({game.Dice.FirstCube}, {game.Dice.SecondCube})");
                Console.WriteLine("Please Enter triangle to move from:");
            }
            return triangle;
        }

        private static int GetFromUserTriangleToMoveInto(IBackgammon game)
        {
            int triangle;
            Console.WriteLine($"You rolled: ({game.Dice.FirstCube}, {game.Dice.SecondCube})");
            Console.WriteLine("Please Enter triangle to move into:");
            while (!int.TryParse(Console.ReadLine(), out triangle))
            {
                Console.Clear();
                DisplayOnConsole(game);
                Console.WriteLine($"Turn: {game.Turn} Player!");
                Console.WriteLine($"You rolled: ({game.Dice.FirstCube}, {game.Dice.SecondCube})");
                Console.WriteLine("Please Enter triangle to move from:");
            }
            return triangle;
        }

        private static void Roll(IBackgammon game)
        {
            if (game.Turn == CheckerType.Black)
            {
                game.BlackPlayerRoll();
            }
            else
            {
                game.WhitePlayerRoll();
            }
        }

        public static void DisplayOnConsole(IBackgammon game)
        {
            Console.WriteLine(" 13 14 15 16 17 18 19 20 21 22 23 24 ");
            MyGameBoard[,] myBoard;
            var max = InitializeMyBoard(game, out myBoard);

            foreach (var triangle in game.Board.Triangles)
            {
                foreach (var checker in triangle.CheckersStack)
                {

                    if (checker.CheckerTriangle > 11)
                    {
                        FillUpperHalfOfTheGameBoardIntoMyGameBoard(myBoard, checker);
                    }
                    else
                    {
                        FillLowerHalfOfTheGameBoardIntoMyGameBoard(max, myBoard, checker);
                    }
                }
            }

            DisplayMyGameBoardOnConsole(max, myBoard);
            Console.WriteLine(" 12 11 10 09 08 07 06 05 04 03 02 01 ");
            DisplayBlackPlayerBarsOnConsole(game);
            DisplayWhitePlayerBarsOnConsole(game);
            Console.WriteLine();
        }

        private static void DisplayWhitePlayerBarsOnConsole(IBackgammon game)
        {
            Console.WriteLine();
            Console.Write("White Dead Bar: ");
            foreach (var ch in game.WhiteDeadCheckersBar)
            {
                Console.Write(" ()");
            }
            Console.Write("   White Outside Bar: ");
            foreach (var ch in game.WhiteOutSideCheckersBar)
            {
                Console.Write(" ()");
            }
            Console.WriteLine();
        }

        private static void DisplayBlackPlayerBarsOnConsole(IBackgammon game)
        {
            Console.WriteLine();
            Console.Write("Black Dead Bar: ");
            foreach (var ch in game.BlackDeadCheckersBar)
            {
                Console.Write(" ()");
            }
            Console.Write("   Black Outside Bar: ");
            foreach (var ch in game.BlackOutSideCheckersBar)
            {
                Console.Write(" ()");
            }
        }

        private static void DisplayMyGameBoardOnConsole(int max, MyGameBoard[,] myBoard)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            for (var i = 0; i < max*2; ++i)
            {
                for (var j = 0; j < 12; ++j)
                {
                    Console.ForegroundColor = myBoard[i, j].FgColor;
                    Console.Write(myBoard[i, j].Value);
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void FillLowerHalfOfTheGameBoardIntoMyGameBoard(int max, MyGameBoard[,] myBoard, IChecker checker)
        {
            var i = max*2 - 1;
            while (myBoard[i, 12 - checker.CheckerTriangle - 1].Value != "   ")
            {
                --i;
            }
            myBoard[i, 12 - checker.CheckerTriangle - 1].Value = " ()";
            myBoard[i, 12 - checker.CheckerTriangle - 1].FgColor = checker.Type == CheckerType.Black
                ? ConsoleColor.Black
                : ConsoleColor.White;
        }

        private static void FillUpperHalfOfTheGameBoardIntoMyGameBoard(MyGameBoard[,] myBoard, IChecker checker)
        {
            var i = 0;
            while (myBoard[i, checker.CheckerTriangle - 12].Value != "   ")
            {
                ++i;
            }
            myBoard[i, checker.CheckerTriangle - 12].Value = " ()";
            myBoard[i, checker.CheckerTriangle - 12].FgColor = checker.Type == CheckerType.Black
                ? ConsoleColor.Black
                : ConsoleColor.White;
        }

        private static int InitializeMyBoard(IBackgammon game, out MyGameBoard[,] myBoard)
        {
            var max = MaxCheckers(game);
            myBoard = new MyGameBoard[max*2, 12];

            for (var i = 0; i < max*2; ++i)
            {
                for (var j = 0; j < 12; ++j)
                {
                    myBoard[i, j] = new MyGameBoard("   ");
                }
            }
            return max;
        }

        private static int MaxCheckers(IBackgammon game)
        {
            return game.Board.Triangles.Select(triangle => triangle.CheckersStack.Count()).Concat(new[] { 0 }).Max();
        }
    }
}
