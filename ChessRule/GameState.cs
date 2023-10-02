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

        // Constructor to initialize any position we want. Useful for testing
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
        }
    }
}
