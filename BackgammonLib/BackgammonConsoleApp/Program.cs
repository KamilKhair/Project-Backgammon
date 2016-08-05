using System;
using System.Linq;
using BackgammonLib;

namespace BackgammonConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var game = new Backgammon();
            while (!game.IsGameOver)
            {
                Console.Clear();
                DisplayOnConsole(game);
                Console.WriteLine($"Turn: {game.Turn} Player!");
                if (game.Dice.Steps == 0)
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
                else
                {
                    int triangle;
                    var move = 0;
                    if (game.BlackDeadCheckersBar.Any() && game.Turn == CheckerType.Black)
                    {
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
                    }
                    else if (game.WhiteDeadCheckersBar.Any() && game.Turn == CheckerType.White)
                    {
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
                    }
                    else
                    {
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
                    }
                    //Thread.Sleep(1000);
                    if (game.Turn == CheckerType.Black)
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
                    else
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
                }
            }
            var winner = game.Winner == CheckerType.Black ? "Black Player" : "White Player";
            Console.WriteLine($"The Winner is {winner}!");
        }

        public static void DisplayOnConsole(Backgammon game)
        {
            Console.WriteLine(" 13 14 15 16 17 18 19 20 21 22 23 24 ");
            var max = MaxCheckers(game);
            var myBoard = new GameB[max * 2, 12];

            for (var i = 0; i < max * 2; ++i)
            {
                for (var j = 0; j < 12; ++j)
                {
                    myBoard[i, j] = new GameB("   ");
                }
            }

            foreach (var triangle in game.Board.AllTriangles)
            {
                foreach (var checker in triangle.Checkers)
                {

                    if (checker.CheckerTraingle > 11)
                    {
                        var i = 0;
                        while (myBoard[i, checker.CheckerTraingle - 12].Value != "   ")
                        {
                            ++i;
                        }
                        myBoard[i, checker.CheckerTraingle - 12].Value = " ()";
                        myBoard[i, checker.CheckerTraingle - 12].FgColor = checker.CheckerType == CheckerType.Black
                            ? ConsoleColor.Black
                            : ConsoleColor.White;
                    }
                    else
                    {
                        var i = max*2 - 1;
                        while (myBoard[i, 12 - checker.CheckerTraingle -1].Value != "   ")
                        {
                            --i;
                        }
                        myBoard[i, 12 - checker.CheckerTraingle - 1].Value = " ()";
                        myBoard[i, 12 - checker.CheckerTraingle - 1].FgColor = checker.CheckerType == CheckerType.Black
                            ? ConsoleColor.Black
                            : ConsoleColor.White;
                    }
                }
            }

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            for (var i = 0; i < max * 2; ++i)
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
            Console.WriteLine(" 12 11 10 09 08 07 06 05 04 03 02 01 ");
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
            Console.WriteLine();
        }

        private static int MaxCheckers(Backgammon game)
        {
            return game.Board.AllTriangles.Select(triangle => triangle.Checkers.Count()).Concat(new[] { 0 }).Max();
        }
    }
}
