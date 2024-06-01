namespace Checkers.MVVM
{
    public class Direction
    {
        public readonly static Direction North = new Direction(-1, 0);
        public readonly static Direction South = new Direction(1, 0);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction West = new Direction(0, -1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = NorthEast + West;
        public readonly static Direction SoutEast = South + East;
        public readonly static Direction SouthWest = South + West;

        public int RowDelta { get; }
        public int ColumnDelta { get; }

        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        public static Direction operator +(Direction direction1, Direction direction2)
        {
            return new Direction(direction1.RowDelta + direction2.RowDelta, direction1.ColumnDelta + direction2.ColumnDelta);
        }

        public static Direction operator *(int scalar, Direction direction)
        {
            return new Direction(scalar * direction.RowDelta, scalar * direction.ColumnDelta);
        }
    }
}
