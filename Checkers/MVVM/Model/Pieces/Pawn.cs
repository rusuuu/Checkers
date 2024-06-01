using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Checkers.MVVM
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }
        public static bool MultipleJumpsMode { get; set; }

        private readonly Direction forward;


        public Pawn(Player color, bool multipleJumpsMode)
        {
            Color = color;

            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else if(color == Player.Black) 
            {
                forward = Direction.South;
            }

            multipleJumpsMode = MultipleJumpsMode;
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color, MultipleJumpsMode);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }

        private bool CanCaptureAt(Position fromPosition, Position capturePosition, Board board)
        {
            if(!Board.IsInside(capturePosition) || board.IsEmpty(capturePosition))
                return false;

            if (board[capturePosition].Color == Color)
                return false;

            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
                if (fromPosition == capturePosition - direction - forward)
                    return true;

            return false;
        }

        private IEnumerable<Move> DiagonalMoves(Position fromPosition, Board board)
        {
            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                Position toPosition = fromPosition + forward + direction;

                if (CanMoveTo(toPosition, board))
                    yield return new NormalMove(fromPosition, toPosition);
            }
        }
        private IEnumerable<Move> DiagonalCaptureMoves(Position fromPosition, Board board)
        {
            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                Position intermediaryPosition = fromPosition + forward + direction;
                Position toPosition = intermediaryPosition + forward + direction;

                if (CanCaptureAt(fromPosition, intermediaryPosition, board) && CanMoveTo(toPosition, board))
                   yield return new CaptureMove(fromPosition, toPosition, intermediaryPosition);
            }
        }

        private IEnumerable<Tuple<Position, List<Position>>> MovePositionInDirections(Position fromPosition, Board board)
        {
            Queue<Tuple<Position, List<Position>>> futurePositions = new Queue<Tuple<Position, List<Position>>>();

            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                List<Position> intermediaryPositions = new List<Position>();
                Position intermediaryPosition = fromPosition + forward + direction;
                Position toPosition = intermediaryPosition + forward + direction;

                if (CanCaptureAt(fromPosition, intermediaryPosition, board) && CanMoveTo(toPosition, board))
                {
                    intermediaryPositions.Add(intermediaryPosition);
                    Tuple<Position, List<Position>> positionsTuple = new Tuple<Position, List<Position>>(toPosition, intermediaryPositions);
                    futurePositions.Enqueue(positionsTuple);
                    yield return positionsTuple;
                }
            }

            if(GameState.MultipleJump == true)
            {
                while (futurePositions.Any())
                {
                    Tuple<Position, List<Position>> queueFirstTuple = futurePositions.Dequeue();
                    fromPosition = queueFirstTuple.Item1;

                    foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
                    {
                        List<Position> intermediaryPositions = [.. queueFirstTuple.Item2];

                        Position intermediaryPosition = fromPosition + forward + direction;

                        Position toPosition = intermediaryPosition + forward + direction;

                        if (CanCaptureAt(fromPosition, intermediaryPosition, board) && CanMoveTo(toPosition, board))
                        {
                            intermediaryPositions.Add(intermediaryPosition);
                            Tuple<Position, List<Position>> positionsTuple = new Tuple<Position, List<Position>>(toPosition, intermediaryPositions);
                            futurePositions.Enqueue(positionsTuple);
                            yield return positionsTuple;
                        }
                    }
                }
            }
        }

        private IEnumerable<MultipleCaptureMove> MultipleDiagonalCaptureMoves(Position fromPosition, Board board)
        {
            return MovePositionInDirections(fromPosition, board).Select(tuple => new MultipleCaptureMove(fromPosition, tuple.Item1, tuple.Item2));
        }
        
        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return DiagonalMoves(fromPosition, board).Concat(MultipleDiagonalCaptureMoves(fromPosition,board));
        }
    }
}