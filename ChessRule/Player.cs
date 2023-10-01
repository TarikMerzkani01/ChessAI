
namespace ChessRule
{
    /*
     * Enum: Player
     * Purpose: Used to store which player's turn it is, and who won the game
     *  also used to represent the color of the chess pieces (REVISE DOC)
     * 
    */
    public enum Player
    {
        None,   // In case of a draw
        White,
        Black
    }

    // Let's add extension method to player
    // Add static class called PlayerExtensions
    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player)
        {
            return player switch
            {
                Player.White => Player.Black,
                Player.Black => Player.White,
                _ => Player.None,
            };
        }
    }

}
