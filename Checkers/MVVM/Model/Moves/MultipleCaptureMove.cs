using System.Collections.Generic;

namespace Checkers.MVVM
{
    public class MultipleCaptureMove : Move
    {
        public override MoveType Type => MoveType.MultipleCaptureMove;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }
        public List<Position> IntermediaryPositions { get; }

        public MultipleCaptureMove(Position fromPosition, Position toPosition, List<Position> intermediaryPositions)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
            IntermediaryPositions = intermediaryPositions;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[FromPosition];
            board[FromPosition] = null;

            foreach(Position intermediaryPosition in IntermediaryPositions) 
            {
                board[intermediaryPosition] = null;
            }

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
