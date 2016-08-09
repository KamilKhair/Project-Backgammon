namespace BackgammonLib
{
    public interface IChecker
    {
        CheckerType Type { get; }
        int CheckerTriangle { get; }
        int CheckerId { get; }
        bool IsAlive { get; }
        bool IsFinished { get; }
    }
}
