using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.MVVM
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        public Piece this[int row, int column]
        {
            get { return pieces[row, column]; }
            set { pieces[row, column] = value; }
        }

        public Piece this[Position position]
        {
            get { return this[position.Row, position.Column]; }
            set { this[position.Row, position.Column] = value; }    
        }
        public Board()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    pieces[row, column] = null;
                }
            }
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        public static Board Initial(int[] pieces)
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            for (int row = 0; row < 3; row++)
            {
                int startColumn = 0;
                if (row % 2 == 0)
                    startColumn = 1;

                for (int column = 0 + startColumn; column < 8; column += 2)
                    this[row, column] = new Pawn(Player.Black, GameState.MultipleJump);
            }

            for (int row = 5; row < 8; row++)
            {
                int startColumn = 0;
                if (row % 2 == 0)
                    startColumn = 1;

                for (int column = 0 + startColumn; column < 8; column += 2)
                    this[row, column] = new Pawn(Player.White, GameState.MultipleJump);
            }
        }

        public static bool IsInside(Position position)
        {
            return position.Row >=0 && position.Row < 8 && position.Column >= 0 && position.Column < 8; 
        }

        public bool IsEmpty(Position position)
        {
            return this[position] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for(int row=-0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                {
                    Position position = new Position(row, column);

                    if(!IsEmpty(position))
                        yield return position;
                }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(position => this[position].Color == player);
        }
    }
}
