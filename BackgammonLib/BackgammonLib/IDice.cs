namespace BackgammonLib
{
    public interface IDice
    {
        CheckerType CurrentType { get; }

        int Steps { get; }

        int FirstCube { get; }

        int SecondCube { get; }

        bool RolledDouble { get; }
    }
}
