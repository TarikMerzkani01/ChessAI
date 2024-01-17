namespace ChessRule
{
    public class PawnPromotion : Move
    {
        // This class will move a pawn to row 0 or 7 (depending of the color), and replace it
        // with either knight, bishop, rook, or queen.
        // Needs Pawn class to generate this kind of move when appropriate

        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly PieceType newType;

        public PawnPromotion(Position from, Position to, PieceType newType)
        {
            FromPos = from;
            ToPos = to;
            this.newType = newType;
        }

        private Piece CreatePromotionPiece(Player color)
        {
            return newType switch
            {
                PieceType.Knight => new Knight(color),
                PieceType.Bishop => new Bishop(color),
                PieceType.Rook => new Rook(color),
                _ => new Queen(color)
            };
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPos];    // gets pawn from Fromposition
            board[FromPos] = null;          // Eliminates pawn in the FromPosition in the board

            Piece promotionPiece = CreatePromotionPiece(pawn.Color);    // Creates promotion Piece
            promotionPiece.HasMoved = true; // Sets boolean value of piece HasMoved to True
            board[ToPos] = promotionPiece;  // Populates ToPos with the newly created PromotionPiece

            return true;
        }
    }
}
