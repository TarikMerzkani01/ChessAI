using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRule
{
    // Class that represents the state of the game
    // Represents the current board configuration
    // which player's turn it is, etc
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }

        public Result Result { get; private set; } = null;

        private int noCaptureOrPawnMoves = 0; // These are half moves

        private string stateString;

        // State history dictionary maps stateStrings from gamestate to frequency
        // (amount of times it has occurred in the duration of the gameState)
        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();

        // Constructor to initialize any position we want. Useful for testing
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;

            stateString = new StateString(CurrentPlayer, board).ToString();
            stateHistory[stateString] = 1;
        }

        // Public method that returns all legal moves for a piece at a given position
        // We can ask the state which moves are legal for a certain piece
        // We pass position since piece object itself doesn't hold that information
        // (piece doesn't know where it is)
        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                // No legal moves if square is empty or if not current player color
                return Enumerable.Empty<Move>(); // Empty enumerable
            }

            // Otherwise, we get piece to obtain legal moves
            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        // Applies move to current Board State
        public void MakeMove(Move move)
        {
            Board.SetPawnSkipPosition(CurrentPlayer, null);
            // Skipped position is forgotten after turn is done
            // (including both half turns)

            bool captureOrPawn = move.Execute(Board);
            if (captureOrPawn)
            {
                noCaptureOrPawnMoves = 0;
                stateHistory.Clear();
            }
            else
            {
                noCaptureOrPawnMoves++;
            }

            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString();
            CheckForGameOver(); // We call this so result is set after a move that ends the game (EXPENSIVE?)
        }   

        // Flow State for UI:
        /* 
         * 1. First, player clicks the piece it wants to move
         * 2. Then, game highlights squares where piece can move to
         * 3. Then, player clicks again on a highlighted piece and moves it there
         * 4. Now, it's the other player's turn
         * 
         * Reminder: 
         * During 2: Only highlight squares of selected piece. 
         *           Only highlight squares if piece is of the player of the current turn.
         *           De-highlight squares if another piece is selected, or if a non-highlighted
         *                  square is selected, or click on same piece to cancel
         * During 3: Remember to de-highlight squares after clicking, remember to transistion to
         *              other player's turn
         * During 4: Same applies as 2, but to other player
         * 
         */

        // LEGALITY OF MOVES AND FLOW OF GAME
        // Legality of moves: Moves are legal if following rules of movement
        //      AND it ensures that the King can't be captured on the next turn by opponent
        // Have to look forward to check legality
        // Checkmate happens when there's no moves available to prevent a King capture
        // Stalemate for no moves but no check? (Also repetition and 50 moves endgame)

        // Condition of Checkmate:
        // King is in check and no legal moves left

        // Condition of Stalemate:
        // Not in check and no legal moves left

        // To detect these, we need a method that recompiles all the legal moves a player can make
        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            // Filter out the illegal ones

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        // Helper method for checking for game over
        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any()) // If no legal moves
            {
                if (Board.IsInCheck(CurrentPlayer)) // If in check, then checkmated and other player won
                {
                    Result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            else if(Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
            else if (ThreefoldRepetition())
            {
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            }
            
        }

        // GameState can detect now
        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            int fullMoves = noCaptureOrPawnMoves / 2;
            return fullMoves == 50;
        }

        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board).ToString();

            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            else
            {
                stateHistory[stateString]++;
            }
        }

        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString] == 3;
        }

    }
}
