using System;
using System.Collections.Generic;
using System.Linq;

namespace BackgammonLib
{
    public class Backgammon : IBackgammon
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

        public event GameFinishedEventHandler GameFinished;
        public event NoAvailableMovesEventHandler NoAvailableMoves;

        protected virtual void OnGameFinished(EventArgs e)
        {
            GameFinished?.Invoke(this, e);
        }

        protected virtual void WhenNoAvailableMoves(NoAvailableMovesEventArgs e)
        {
            NoAvailableMoves?.Invoke(this, e);
        }

        internal void RaiseGameFinishedEvent()
        {
            OnGameFinished(EventArgs.Empty);
        }

        internal void RaiseNoAvailableMovesEvent(int firstCube, int secondCube)
        {
            WhenNoAvailableMoves(new NoAvailableMovesEventArgs(firstCube, secondCube));
        }

        public CheckerType Turn { get; internal set; }

        public bool IsGameOver => GameOver;

        public CheckerType Winner { get; internal set; }

        public Dice Dice => GameDice;
        public IGameBoard Board => GameBoard;

        public int BlackPlayerCurrentCheckers
        {
            get
            {
                return GameBoard.Triangles.SelectMany(p => p.CheckersStack).Count(ch => ch.Type == CheckerType.Black && !ch.IsFinished);
            }
        }
        public int WhitePlayerCurrentCheckers
        {
            get
            {
                return GameBoard.Triangles.SelectMany(p => p.CheckersStack).Count(ch => ch.Type == CheckerType.White && !ch.IsFinished);
            }
        }
        public IEnumerable<IChecker> BlackDeadCheckersBar => BlackPlayer.DeadCheckersBar.Bar;
        public IEnumerable<IChecker> WhiteDeadCheckersBar => WhitePlayer.DeadCheckersBar.Bar;
        public IEnumerable<IChecker> BlackOutSideCheckersBar => BlackPlayer.OutSideCheckersBar.Bar;
        public IEnumerable<IChecker> WhiteOutSideCheckersBar => WhitePlayer.OutSideCheckersBar.Bar;

        public bool AllBlackCheckersInLocalArea => BlackPlayer.CheckIfThereAreAvailableMoves();
        public bool AllWhiteCheckersInLocalArea => WhitePlayer.CheckIfThereAreAvailableMoves();

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

        internal readonly Dice GameDice;
        internal readonly IGameBoard GameBoard;
        internal bool GameOver = false;
        internal readonly Player BlackPlayer;
        internal readonly Player WhitePlayer;
    }
}
