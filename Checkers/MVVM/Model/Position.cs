using System.Collections.Generic;

namespace Checkers.MVVM
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {  
            Row = row; 
            Column = column; 
        }

        public Player SquareColor(int row, int column)
        {
            if (row % 2 == column % 2)
                return Player.White;
           
            return Player.Black;
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            int hashCode = 240067226;
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            hashCode = hashCode * -1521134295 + Column.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator +(Position position, Direction direction)
        {
            return new Position(position.Row + direction.RowDelta, position.Column + direction.ColumnDelta);
        }

        public static Position operator -(Position position, Direction direction)
        {
            return new Position(position.Row - direction.RowDelta, position.Column - direction.ColumnDelta);
        }
    }
}
