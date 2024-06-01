using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Checkers.MVVM
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;
        public abstract Piece Copy();
        public abstract IEnumerable<Move> GetMoves(Position fromPosition, Board board);
    }
}
