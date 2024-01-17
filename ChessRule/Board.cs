using System.Drawing;

namespace ChessRule
{
    public class Board
    {
        // 2D Array for storing all the pieces
        // This represents every square in the board
        // Reminder pieces can be None (no piece)
        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.White, null},
            { Player.Black, null}
        };

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

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }
        
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

        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVKing(counting) || IsKingBishopVKing(counting) ||
                   IsKingKnightVKing(counting) || IsKingBishopVKingBishop(counting);
        }


        // All Insufficient Material scenarios
        // King v King
        // King, Bishop v King
        //
        private static bool IsKingVKing(Counting counting)  
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVKingBishop(Counting counting)
        {
            // These pieces are insufficient for checkmate if both bishops are on the same colored squares
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            // Locate bishops and see if they are same colored square bishops

            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos))
            {
                return false;
            }

            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook &&
                   !king.HasMoved && !rook.HasMoved;

            // By the way, no need to check types. Assuming method is called with the initial
            // position of king and rook, and the pieces are completely unmoved,
            // means they must have the correct types
            // For clarity, we leave code as is
        }

        public bool CastleRightKS(Player player)
        {
            // Returns true if the given player has the right to castleks
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        public bool CastleRightQS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        // Check if player can capture EnPassant

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            // Returns true if the given player has a pawn in position to capture EnPassant
            foreach (Position pos in pawnPositions.Where(IsInside))     // Here's the error checking for bounds :D
            {
                Piece piece = this[pos];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                // If here, then there IS a pawn of correct color in correct pos to EnPassant
                // We don't know if it's legal to make though (leaves king in check after EnPassant)
                // To test that, we create an EnPassant move

                EnPassant move = new EnPassant(pos, skipPos);
                if (move.IsLegal(this))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent());

            if (skipPos == null)
            {
                return false;
            }

            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] {skipPos + Direction.SouthWest, skipPos + Direction.SouthEast},
                Player.Black => new Position[] {skipPos + Direction.NorthWest, skipPos + Direction.NorthEast},
                _ => Array.Empty<Position>()
            };

            return HasPawnInPosition(player, pawnPositions, skipPos);

        }

        // Now we can check player's castling rights and whether they can capture EnPassant or not in a given turn


        // Added by Tarik v/////////////////////////////////////////////////
        public static Board CustomInitial(String fenString)
        {
            Board newBoard = new Board();
            newBoard.CustomAddPieces(fenString);
            return newBoard;
        }

        private void CustomAddPieces(String fenString)
        {
            int currow = 0;
            int curcol = 0; // Starts UpperLeft Corner
            foreach (char ch in fenString)
            {
                
                if (Char.IsNumber(ch))
                {
                    // We add an offset in this case
                    curcol += ch - '0';
                } 
                else if (ch == '/')
                {
                    curcol = 0;
                    currow++;
                } 
                else if (currow > 7 || curcol < 0 || curcol > 7)
                {
                    // Case to prevent non-correct FenStrings
                    Console.WriteLine("Incorrectly formatted FEN String. Does not represent real board position");
                    break;
                }
                else
                {
                    // If piece. We use a switch case for this
                    Player pieceColor = Player.White;
                    char charpiece = char.ToLower(ch);
                    if (charpiece == ch)
                    {
                        // If Lowercase
                        pieceColor = Player.Black;
                    }
                    _ = charpiece switch
                    {
                        'p' => this[currow, curcol] = new Pawn(pieceColor),/////////////
                        'n' => this[currow, curcol] = new Knight(pieceColor),
                        'b' => this[currow, curcol] = new Bishop(pieceColor),
                        'r' => this[currow, curcol] = new Rook(pieceColor),
                        'q' => this[currow, curcol] = new Queen(pieceColor),
                        'k' => this[currow, curcol] = new King(pieceColor),
                        _ => throw new NotImplementedException("Incorrect formatted FEN String"),
                    };
                    curcol++;
                }
            }
        }

        // Return a data structure that represents a bitboard


        // Added by Tarik ^/////////////////////////////////////////////////

    }
}
