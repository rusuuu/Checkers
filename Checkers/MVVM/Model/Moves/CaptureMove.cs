namespace Checkers.MVVM
{
    internal class CaptureMove : Move
    {
        public override MoveType Type => MoveType.CaptureMove;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }
        public Position IntermediaryPosition { get; }

        public CaptureMove(Position fromPosition, Position toPosition, Position intermediaryPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            IntermediaryPosition = intermediaryPosition;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[FromPosition];
            board[FromPosition] = null;
            board[IntermediaryPosition] = null;
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
