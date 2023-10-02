using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessRule;

namespace ChessUI
{
    // Purpose of this class is to load the images from the Assets folder
    // and make it convenient to access them
    // Uses ChessRule namespace
    public static class Images
    {
        // Store images in two dictionaries, one for white and one for black

        // Key is a PieceType, and Value is ImageSource.
        // Idea is that we can conveniently look up the correct image for a certain kind of piece
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnW.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopW.png") },
            {PieceType.Knight, LoadImage("Assets/KnightW.png") },
            {PieceType.Rook, LoadImage("Assets/RookW.png") },
            {PieceType.Queen, LoadImage("Assets/QueenW.png") },
            {PieceType.King, LoadImage("Assets/KingW.png") },
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnB.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopB.png") },
            {PieceType.Knight, LoadImage("Assets/KnightB.png") },
            {PieceType.Rook, LoadImage("Assets/RookB.png") },
            {PieceType.Queen, LoadImage("Assets/QueenB.png") },
            {PieceType.King, LoadImage("Assets/KingB.png") },
        };

        private static ImageSource LoadImage(string filePath)
        {
            // Takes relative path of an image as paramater
            // We return a new BitmapImage, where we pass a new Uri with the filePath,
            // and also specifying that it is a relative path
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        // Helper method GetImage,
            // Takes color and type
        public static ImageSource GetImage(Player color, PieceType type)
        {
            return color switch
            {
                Player.White => whiteSources[type],
                Player.Black => blackSources[type],
                _ => null
            }; 
        }
        
            // Takes piece
        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return GetImage(piece.Color, piece.Type);
        }
        
    }
}
