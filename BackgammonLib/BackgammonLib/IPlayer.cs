using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal interface IPlayer
    {
        bool IsWhitePlayerCanPlay { get; set; }
        bool IsBlackPlayerCanPlay { get; set; }

        bool Roll();

        bool Move(int triangle, int move);

        bool MoveFromDeadBar(int move);

        bool CheckIfThereAreAvailableMoves();

        bool CheckIfCanMove(int triangle, int move);

        bool CheckIfCanMoveFromDeadBar(int triangle);
    }
}
