using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRule
{   
    //Represents a position on the board (a single square)
    //(0,0) is upper left corner square
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Player SquareColor()
        {
            if ((Row + Column) % 2 == 0)
                return Player.White;
            return Player.Black;
        }

        // We override equals and get hashcode so the position
        // class can be used as the key in a dictionary
        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        // Let's overload the + operator, so we can add a Direction to a Position,
        // such that it returns a new Position object of the move given.

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }

        // Example:

        //Position from = new Position(0, 4);
        //Position to = from + 3 * Direction.SouthEast;



    }
}
