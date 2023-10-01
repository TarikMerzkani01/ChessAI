namespace ChessRule
{
    public class Direction
    {
        // All pieces move in a certain direction
        // Convenient For generating MOVES

        // Having the base of the class (members and methods),
        //  we can create the directions we need

        // UpDownLeftRight
        public readonly static Direction North = new Direction(-1, 0);
        public readonly static Direction South = new Direction(1, 0);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction West = new Direction(0, -1);

        // Diagonals

        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;


        public int RowDelta { get; }
        public int ColumnDelta { get; }

        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        // Override plus operator

        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
        }

        // Override multiplication operator, so Directions are scalable
        public static Direction operator *(int scalar, Direction dir1)
        {
            return new Direction(dir1.RowDelta * scalar, dir1.ColumnDelta * scalar);
        }

    }
}
