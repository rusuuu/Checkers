namespace Checkers.MVVM
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        public NormalMove(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[FromPosition];
            board[FromPosition] = null;
            board[ToPosition] = piece;

            if (piece.Type == PieceType.Pawn && (ToPosition.Row == 0 || ToPosition.Row == 7))
            {
                Piece promotionPiece = new King(piece.Color, GameState.MultipleJump);
                promotionPiece.HasMoved = true;
                board[ToPosition] = promotionPiece;
            }
            else
                piece.HasMoved = true;
        }
    }
}
