using System.Collections.Generic;

namespace BackgammonLib
{
    public interface IBackgammon
    {
        CheckerType Turn { get; }

        bool IsGameOver { get; }

        CheckerType Winner { get; }

        IDice Dice { get; }

        IGameBoard Board { get; }

        int BlackPlayerCurrentCheckers { get; }

        int WhitePlayerCurrentCheckers { get; }

        IEnumerable<IChecker> BlackDeadCheckersBar { get; }

        IEnumerable<IChecker> WhiteDeadCheckersBar { get; }

        IEnumerable<IChecker> BlackOutSideCheckersBar { get; }

        IEnumerable<IChecker> WhiteOutSideCheckersBar { get; }

        bool AllBlackCheckersInLocalArea { get; }

        bool AllWhiteCheckersInLocalArea { get; }

        bool BlackPlayerRoll();

        bool WhitePlayerRoll();

        bool MoveBlack(int triangle, int move);

        bool MoveBlackFromDeadBar(int move);

        bool MoveWhite(int triangle, int move);

        bool MoveWhiteFromDeadBar(int move);

        event GameFinishedEventHandler GameFinished;

        event NoAvailableMovesEventHandler NoAvailableMoves;
    }
}
