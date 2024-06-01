using Checkers.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.MVVM
{
    public class GameState
    {
        public Board Board { get; set; }
        public Player CurrentPlayer { get; set; }
        public Result Result { get; private set; } = null;
        public static bool MultipleJump { get; set; }

        public GameState()
        {
            Board = new Board();
        }

        public GameState(Board board, Player currentPlayer, bool multipleJump)
        {
            Board = board;
            CurrentPlayer = currentPlayer;
            MultipleJump = multipleJump;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position position)
        {
            if(Board.IsEmpty(position) || Board[position].Color!=CurrentPlayer)
                return Enumerable.Empty<Move>();

            Piece piece = Board[position];
            return piece.GetMoves(position, Board);
        }

        public void MakeMove(Move move)
        {
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllMovesForPlayer(Player player)
        {
            IEnumerable<Move> moves = Board.PiecePositionsFor(player).SelectMany(position =>
            {
                Piece piece = Board[position];
                return piece.GetMoves(position, Board);
            });

            return moves;
        }

        private void CheckForGameOver()
        {
            if(!AllMovesForPlayer(CurrentPlayer).Any())
            {
                Result=Result.Win(CurrentPlayer.Opponent());
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        public GameStateToSerialize PrepareSerialization(string saveName)
        {
            List<int> pieces = new List<int>();

            int currentPlayer = 1;
            if (CurrentPlayer == Player.Black)
                currentPlayer = 2;

            int index = 0;
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                {
                    int piece=0;

                    if (Board[row, column] == null)
                        piece = 0;
                    else
                    {
                        switch (Board[row, column].Type)
                        {
                            case PieceType.Pawn when Board[row, column].Color == Player.White:
                                piece = 1;
                                break;
                            case PieceType.King when Board[row, column].Color == Player.White:
                                piece = 2;
                                break;
                            case PieceType.Pawn when Board[row, column].Color == Player.Black:
                                piece = 3;
                                break;
                            case PieceType.King when Board[row, column].Color == Player.Black:
                                piece = 4;
                                break;
                            default:
                                break;
                        }
                    }

                    pieces.Add(piece);
                    index++;
                }

            int multipleJump = 0;
            if (MultipleJump == true)
                multipleJump = 1;

            return new GameStateToSerialize(saveName, pieces, currentPlayer,multipleJump);
        }

        public void PrepareDeserialization(GameStateToSerialize gameStateToSerialize)
        {
            if (gameStateToSerialize.playerToMove == 1)
                CurrentPlayer = Player.White;
            else
                CurrentPlayer = Player.Black;

            List<int> pieces = gameStateToSerialize.Pieces;
            int index = 0;
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                {
                    switch (pieces[index])
                    {
                        case 1:
                            Board[row, column] = new Pawn(Player.White, GameState.MultipleJump);
                            break;
                        case 2:
                            Board[row, column] = new King(Player.White, GameState.MultipleJump);
                            break;
                        case 3:
                            Board[row, column] = new Pawn(Player.Black, GameState.MultipleJump);
                            break;
                        case 4:
                            Board[row, column] = new King(Player.Black, GameState.MultipleJump);
                            break;
                        default:
                            break;
                    }

                    index++;
                }

            if (gameStateToSerialize.multipleJump == 0)
                MultipleJump = false;
            else
                MultipleJump = true;
        }
    }
}