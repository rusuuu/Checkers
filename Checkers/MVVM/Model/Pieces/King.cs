using Checkers.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.MVVM
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        public static bool MultipleJumpsMode { get; set; }

        public King(Player color, bool multipleJumpsMode)
        {
            Color = color;
            MultipleJumpsMode = multipleJumpsMode;
        }

        public override Piece Copy()
        {
            King copy = new King(Color, MultipleJumpsMode);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }

        private bool CanCaptureAt(Position fromPosition, Position capturePosition, Board board)
        {
            if (!Board.IsInside(capturePosition) || board.IsEmpty(capturePosition))
                return false;

            if (board[capturePosition].Color == Color)
                return false;

            foreach (Direction verticalDirection in new Direction[] { Direction.North, Direction.South })
                foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
                    if (fromPosition == capturePosition - verticalDirection - horizontalDirection)
                        return true;

            return false;
        }

        private IEnumerable<Move> DiagonalMoves(Position fromPosition, Board board)
        {
            foreach (Direction verticalDirection in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
                {
                    Position toPosition = fromPosition + verticalDirection + horizontalDirection;

                    if (CanMoveTo(toPosition, board))
                    {
                        yield return new NormalMove(fromPosition, toPosition);
                    }
                }
            }
               
        }
        private IEnumerable<Move> DiagonalCaptureMoves(Position fromPosition, Board board)
        {
            foreach (Direction verticalDirection in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
                {
                    Position intermediaryPosition = fromPosition + verticalDirection + horizontalDirection;
                    Position toPosition = intermediaryPosition + verticalDirection + horizontalDirection;

                    if (CanCaptureAt(fromPosition, intermediaryPosition, board) && CanMoveTo(toPosition, board))
                    {
                        yield return new CaptureMove(fromPosition, toPosition, intermediaryPosition);
                    }
                }
            }
        }

        private IEnumerable<Tuple<Position, List<Position>>> MovePositionInDirections(Position fromPosition, Board board)
        {
            Queue<Tuple<Position, List<Position>>> futurePositions = new Queue<Tuple<Position, List<Position>>>();

            foreach (Direction verticalDirection in new Direction[] {Direction.North, Direction.South })
            {
                foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
                {
                    List<Position> intermediaryPositions = new List<Position>();
                    Position intermediaryPosition = fromPosition + verticalDirection + horizontalDirection;
                    Position toPosition = intermediaryPosition + verticalDirection + horizontalDirection;

                    if (CanCaptureAt(fromPosition, intermediaryPosition, board) && CanMoveTo(toPosition, board))
                    {
                        intermediaryPositions.Add(intermediaryPosition);
                        Tuple<Position, List<Position>> positionsTuple = new Tuple<Position, List<Position>>(toPosition, intermediaryPositions);
                        futurePositions.Enqueue(positionsTuple);
                        yield return positionsTuple;
                    }
                }
            }

            if(GameState.MultipleJump == true)
            {
                while (futurePositions.Any())
                {
                    Tuple<Position, List<Position>> queueFirstTuple = futurePositions.Dequeue();
                    fromPosition = queueFirstTuple.Item1;

                    foreach (Direction verticalDirection in new Direction[] { Direction.North, Direction.South })
                    {
                        foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
                        {
                            List<Position> intermediaryPositions = [.. queueFirstTuple.Item2];

                            Position intermediaryPosition = fromPosition + verticalDirection + horizontalDirection;

                            Position toPosition = intermediaryPosition + verticalDirection + horizontalDirection;

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
        }

        private IEnumerable<MultipleCaptureMove> MultipleDiagonalCaptureMoves(Position fromPosition, Board board)
        {
            return MovePositionInDirections(fromPosition, board).Select(tuple => new MultipleCaptureMove(fromPosition, tuple.Item1, tuple.Item2));
        }

        public override IEnumerable<Move> GetMoves(Position fromPosition, Board board)
        {
            return DiagonalMoves(fromPosition, board).Concat(MultipleDiagonalCaptureMoves(fromPosition, board));
        }
    }
}