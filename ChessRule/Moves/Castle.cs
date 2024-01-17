using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessRule
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly Position rookFromPos;
        private readonly Position rookToPos;

        public Castle(MoveType type, Position kingPos)
        {
            // Takes movetype (either castlekingside or castlequeenside)
            // and kingPosition (which is always the initial start position)
            Type = type;
            FromPos = kingPos;

            if (type == MoveType.CastleKS)
            {
                kingMoveDir = Direction.East;
                ToPos = new Position(kingPos.Row, 6);
                rookFromPos = new Position(kingPos.Row, 7);
                rookToPos = new Position(kingPos.Row, 5);
            }
            else if (type == MoveType.CastleQS)
            {
                kingMoveDir = Direction.West;
                ToPos = new Position(kingPos.Row, 2);
                rookFromPos = new Position(kingPos.Row, 0);
                rookToPos = new Position(kingPos.Row, 3);
            }
        }

        public override bool Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);

            return false;
        }

        // We overrite the method IsLegal since we have to check if castling is done
        // through check or into check, which would be illegal

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;
            if (board.IsInCheck(player))    //if already in check
            {
                return false;
            }

            Board copy = board.Copy();
            Position KingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(KingPosInCopy, KingPosInCopy + kingMoveDir).Execute(copy);
                KingPosInCopy += kingMoveDir;   //keep updating the kingposincopy 
                // this is why kingmovedir is used (different for ks and qs castling)

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
