
namespace ChessRule
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason EndReason { get; }

        public Result(Player winner, EndReason reason)
        {
            Winner = winner;
            EndReason = reason;
        }

        // Convenient methods for creating a result: Win and Draw

        // Returns a new result containing winning player and checkmate reason
        public static Result Win(Player winner)
        {
            return new Result(winner, EndReason.Checkmate);
        }


        // Returns a draw result. No player, just result
        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    }
}
