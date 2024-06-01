using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Checkers.MVVM
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new() 
        {
            {PieceType.Pawn, LoadImage("Resources/WhitePawnPiece.png") },
            {PieceType.King, LoadImage("Resources/WhiteKingPiece.png")}
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Pawn, LoadImage("Resources/BlackPawnPiece.png") },
            {PieceType.King, LoadImage("Resources/BlackKingPiece.png")}
        };

        private static ImageSource LoadImage(string filepath)
        {
            return new BitmapImage(new Uri(filepath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType pieceType)
        {
            return color switch
            {
                Player.White => whiteSources[pieceType],
                Player.Black => blackSources[pieceType],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if(piece == null) return null;

            return GetImage(piece.Color, piece.Type);
        }
    }
}
