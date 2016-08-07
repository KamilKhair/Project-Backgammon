using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgammonLib
{
    public class Backgammon
    {
        public Backgammon()
        {
            GameBoard = new GameBoard();
            GameDice = new Dice(this);
            Turn = CheckerType.Black;
            Winner = CheckerType.None;
            BlackPlayer = new BlackPlayer(CheckerType.Black, this);
            WhitePlayer = new WhitePlayer(CheckerType.Black, this);
        }

        internal readonly Dice GameDice;
        internal readonly GameBoard GameBoard;
        internal bool GameOver = false;
        internal readonly Player BlackPlayer;
        internal readonly Player WhitePlayer;

        public event GameFinishedEventHandler GameFinished;
        public event NoAvailableMovesEventHandler NoAvailableMoves;

        protected virtual void OnGameFinished(EventArgs e)
        {
            GameFinished?.Invoke(this, e);
        }

        protected virtual void WhwnNoAvailableMoves(EventArgs e)
        {
            NoAvailableMoves?.Invoke(this, e);
        }

        internal void RaiseGameFinishedEvent()
        {
            OnGameFinished(EventArgs.Empty);
        }

        internal void RaiseNoAvailableMovesEvent()
        {
            WhwnNoAvailableMoves(EventArgs.Empty);
        }

        public CheckerType Turn { get; internal set; }

        public bool IsGameOver => GameOver;

        public CheckerType Winner { get; internal set; }

        public Dice Dice => GameDice;

        public GameBoard Board => GameBoard;

        public int BlackPlayerCheckers
        {
            get
            {
                return GameBoard.Triangles.SelectMany(p => p.CheckersStack).Count(ch => ch.Type == CheckerType.Black && !ch.IsFinished);
            }
        }

        public bool IsBlackPlayerCanPlay
        {
            get
            {
                var count = 0;
                Parallel.For(18, 24, (i) =>
                {
                    if (GameBoard.Triangles[i].Type == CheckerType.White && GameBoard.Triangles[i].CheckersCount > 1)
                    {
                        Interlocked.Increment(ref count);
                    }
                });
                return count != 6;
            }
        }

        public int WhitePlayerCheckers
        {
            get
            {
                return GameBoard.Triangles.SelectMany(p => p.CheckersStack).Count(ch => ch.Type == CheckerType.White && !ch.IsFinished);
            }
        }

        public bool IsWhitePlayerCanPlay
        {
            get
            {
                var count = 0;
                Parallel.For(0, 6, (i) =>
                {
                    if (GameBoard.Triangles[i].Type == CheckerType.Black && GameBoard.Triangles[i].CheckersCount > 1)
                    {
                        Interlocked.Increment(ref count);
                    }
                });
                return count != 6;
            }
        }

        public IEnumerable<Checker> BlackDeadCheckersBar => BlackPlayer.DeadCheckersBar.Bar;
        public IEnumerable<Checker> WhiteDeadCheckersBar => WhitePlayer.DeadCheckersBar.Bar;
        public IEnumerable<Checker> BlackOutSideCheckersBar => BlackPlayer.OutSideCheckersBar.Bar;
        public IEnumerable<Checker> WhiteOutSideCheckersBar => WhitePlayer.OutSideCheckersBar.Bar;

        public bool BlackPlayerRoll()
        {
            return Turn == CheckerType.Black && BlackPlayer.Roll();
        }

        public bool WhitePlayerRoll()
        {
            return Turn == CheckerType.White && BlackPlayer.Roll();
        }

        public bool MoveBlack(int triangle, int move)
        {
            return Turn == CheckerType.Black && BlackPlayer.Move(triangle, move);
        }

        public bool MoveBlackFromDeadBar(int move)
        {
            return Turn == CheckerType.Black && BlackPlayer.MoveFromDeadBar(move);
        }

        public bool MoveWhite(int triangle, int move)
        {
            return Turn == CheckerType.White && WhitePlayer.Move(triangle, move);
        }

        public bool MoveWhiteFromDeadBar(int move)
        {
            return Turn == CheckerType.White && WhitePlayer.MoveFromDeadBar(move);
        }
    }
}
