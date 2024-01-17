namespace ChessRule
{
    public class Pawn : Piece   // Implements Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        private readonly Direction forward; // A direction we will call forward

        // Constructor
        public Pawn(Player color)
        {
            Color = color;
            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }

        // Technically a clone method
        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // Made static since it does not access any member variables
        // not required but done whenever this is the case
        private static bool CanMoveTo(Position pos, Board board)
        {
            // Returns true iff pawn can move forward in given pos
            // Must be inside Board, must be empty
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        // Similar to canMove, we add canCaptureAt
        // Returns true if pawn can move in diagonally into the poition
        private bool CanCaptureAt(Position pos, Board board)
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                // if not inside, or no piece to capture, then false
                return false;
            }

            return board[pos].Color != Color;
        }

        // Helper method that creates 4 promotion moves
        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Queen);
        }


        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            // Check if can move one step, then we take care of two steps
            // when it's the pawn's first time moving

            Position oneMovePos = from + forward;

            if (CanMoveTo(oneMovePos, board))
            {
                if (oneMovePos.Row == 0 || oneMovePos.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        // We just return all 4 promotion moves
                        yield return promMove;
                    }
                } else
                {
                    // create normal move which moves pawn to that position
                    yield return new NormalMove(from, oneMovePos);
                }

                Position twoMovesPos = oneMovePos + forward;

                //if it hasn't moved yet and can move two squares forward
                if (!HasMoved && CanMoveTo(twoMovesPos, board))
                {
                    // for now, use NormalMove. Won't be the case later
                    // Will change when we handle En Passant
                    yield return new DoublePawn(from, twoMovesPos);

                    // We use DoublePawn since DoublePawn stores the skipped position
                }
            }
        }

        // Method for diagonal moves (returns all move that result in capture for pawn)
        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            // We loop over two directions, west and east
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + forward + dir; // Goes diagonally left or right
                // North or South depending on forward

                if (to == board.GetPawnSkipPosition(Color.Opponent()))
                {
                    // En Passant move!! But we still need to tweak because it's only possible on first turn
                    yield return new EnPassant(from, to);
                }
                else if (CanCaptureAt(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            // We just return all 4 promotion moves
                            yield return promMove;
                        }
                    }
                    else
                    {
                        // create normal move which moves pawn to that position
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Concatenates forward moves (moves forward)
            // with possible captures (Diagonal moves)
            // for pawn
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Check only if a diagonal move would capture the opponent's king
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }



        // To-Do: Pawn Promotion and EnPassant


    }
}
