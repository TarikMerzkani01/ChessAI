namespace ChessRule
{
    public abstract class Move
    {
        // Base Class for all concrete moves 
        // (Interface to be implemented, hence abstract)
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }

        public abstract bool Execute(Board board);
        // Execute the move on the board it takes as parameter
        // This is a bit like the design pattern: Command Pattern

        // Returns True IFF executing the move does not result in the player's king being in check
        // Virtual since this will be different for Castling Moves
        public virtual bool IsLegal(Board board)
        {
            // How can we tell, we use the methods we've written
            // If a move is legal, we make a copy of the board and execute move on copy
            // Move is legal IFF the moving player's king is not in check (can be captured) after the move

            // Copy only exists for a sec (only at the scope of this function, then deleted)

            Player player = board[FromPos].Color;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(player); // If NOT IN CHECK AFTER move, return True
                                                 // 
            // This is simple and correct implementation, but not the most efficient
            // If we decide to add computer controlled players, we might need something faster or more efficient
        }

    }
}
