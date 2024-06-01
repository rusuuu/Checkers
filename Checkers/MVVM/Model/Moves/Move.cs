namespace Checkers.MVVM
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPosition { get; }
        public abstract Position ToPosition { get; }
        public abstract void Execute(Board board);
    }
}
