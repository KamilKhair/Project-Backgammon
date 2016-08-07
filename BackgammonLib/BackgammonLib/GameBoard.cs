using System.Collections.Generic;

namespace BackgammonLib
{
    public class GameBoard
    {
        internal GameBoard()
        {
            Triangles = new List<Triangle>();
            InitializeBoard();
        }

        internal List<Triangle> Triangles;
        private const int NumOfPoints = 24;

        public IEnumerable<Triangle> AllTriangles => Triangles;

        private void InitializeBoard()
        {
            for (var i = 0; i < NumOfPoints; ++i)
            {
                switch (i)
                {
                    case 0:
                        Triangles.Add(new Triangle(CheckerType.White, i, 0,  false, 2));
                        break;
                    case 5:
                        Triangles.Add(new Triangle(CheckerType.Black, i, 2, false, 5));
                        break;
                    case 7:
                        Triangles.Add(new Triangle(CheckerType.Black, i, 7, false, 3));
                        break;
                    case 11:
                        Triangles.Add(new Triangle(CheckerType.White, i, 10, false, 5));
                        break;
                    case 12:
                        Triangles.Add(new Triangle(CheckerType.Black, i, 15, false, 5));
                        break;
                    case 16:
                        Triangles.Add(new Triangle(CheckerType.White, i, 20, false, 3));
                        break;
                    case 18:
                        Triangles.Add(new Triangle(CheckerType.White, i, 23, false, 5));
                        break;
                    case 23:
                        Triangles.Add(new Triangle(CheckerType.Black, i, 28, false, 2));
                        break;
                    default:
                        Triangles.Add(new Triangle(CheckerType.None, i, int.MaxValue));
                        break;
                }
            }
        }
    }
}
