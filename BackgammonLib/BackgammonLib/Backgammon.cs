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
            _board = new GameBoard(this);
        }

        internal readonly GameBoard _board;
        internal readonly Dice _dice = new Dice();
        internal readonly Player _blackPlayer = new BlackPlayer("Black Player", CheckerType.Black);
        internal readonly Player _whitePlayer = new WhitePlayer("White Player", CheckerType.White);
        internal bool GameOver = false;
        internal CheckerType _winner = CheckerType.None;
        internal CheckerType _turn = CheckerType.Black;

        public CheckerType Turn
        {
            get { return _turn; }
            internal set { _turn = value; }
        }

        public bool IsGameOver => GameOver;

        public CheckerType Winner => _winner;

        public Dice Dice => _dice;

        public GameBoard Board => _board;

        public int BlackPlayerCheckers
        {
            get
            {
                return _board.Triangles.SelectMany(p => p.CheckersStack).Count(ch => ch.Type == CheckerType.Black && !ch.IsFinished);
            }
        }

        public bool IsBlackPlayerCanPlay
        {
            get
            {
                var count = 0;
                Parallel.For(18, 24, (i) =>
                {
                    if (_board.Triangles[i].Type == CheckerType.White)
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
                return _board.Triangles.SelectMany(p => p.CheckersStack).Count(ch => ch.Type == CheckerType.White && !ch.IsFinished);
            }
        }

        public bool IsWhitePlayerCanPlay
        {
            get
            {
                var count = 0;
                Parallel.For(0, 6, (i) =>
                {
                    if (_board.Triangles[i].Type == CheckerType.Black)
                    {
                        Interlocked.Increment(ref count);
                    }
                });
                return count != 6;
            }
        }

        public IEnumerable<Checker> BlackDeadCheckersBar => _board.DeadCheckersBarBlack.Bar;
        public IEnumerable<Checker> WhiteDeadCheckersBar => _board.DeadCheckersBarWhite.Bar;
        public IEnumerable<Checker> BlackOutSideCheckersBar => _board.OutSideCheckersBarBlack.Bar;
        public IEnumerable<Checker> WhiteOutSideCheckersBar => _board.OutSideCheckersBarrWhite.Bar;

        public bool BlackPlayerRoll()
        {
            return _turn == CheckerType.Black && _blackPlayer.Roll(this);
        }

        public bool WhitePlayerRoll()
        {
            return _turn == CheckerType.White && _blackPlayer.Roll(this);
        }

        public bool MoveBlack(int triangle, int move)
        {
            return Turn == CheckerType.Black && _blackPlayer.Move(this, triangle, move);
        }

        public bool MoveBlackFromDeadBar(int move)
        {
            return Turn == CheckerType.Black && _blackPlayer.MoveFromDeadBar(this, move);
        }

        public bool MoveWhite(int triangle, int move)
        {
            return Turn == CheckerType.White && _whitePlayer.Move(this, triangle, move);
        }

        public bool MoveWhiteFromDeadBar(int move)
        {
            return Turn == CheckerType.White && _whitePlayer.MoveFromDeadBar(this, move);
        }
    }
}
