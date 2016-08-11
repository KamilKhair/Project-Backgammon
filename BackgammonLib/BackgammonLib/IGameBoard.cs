using System.Collections.Generic;

namespace BackgammonLib
{
    public interface IGameBoard
    {
        List<ITriangle> Triangles { get; }
    }
}
