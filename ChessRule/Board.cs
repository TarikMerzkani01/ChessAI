namespace ChessRule
{
    public class Board
    {
        // 2D Array for storing all the pieces
        // This represents every square in the board
        // Reminder pieces can be None (no piece)
        private readonly Piece[,] pieces = new Piece[8, 8];

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        // Now board itself can be accessed like a 2d array
        // Board board = new Board();
        // board[7,4] = new King(Player.White);
        // Piece piece = board[7,4];

        // Also convenient to use a Position object as index

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        // Can now get and set a piece at a given Square either by
        // row and column or by Position object

        // Default constructor already sets up Board with no pieces
        // Thus, we just add a static method to initialize a start game board

        public Board()
        {

        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(Player.Black);
                this[6, i] = new Pawn(Player.White);
            }
        }       

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Column >= 0 && pos.Row < 8 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        // Helper method PiecePositions returns all non-empty Piece Positions

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);

                    if (!IsEmpty(pos))  // If Not Empty
                    {
                        yield return pos;
                    }
                }
            }
        }

        // Same helper but for a specified player
        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            // Remember, this == board instance, which is accessible as a 2d array or via Position object
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        // Finally, a Check Detection function
        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy(); // Makes a copy of piece in thisboard, then places it on copy board
            }

            return copy;
        }

    }
}
