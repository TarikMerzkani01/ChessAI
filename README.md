# Chess AI Solution by Tarik Merzkani

> Start Date: 9/28/2023

Credits for UI and Chess game codework obtained from code-along by OttoBotCode at the link below:<br>
https://www.youtube.com/playlist?list=PLFk1_lkqT8MahHPi40ON-jyo5wiqnyHsL 

## Additions in the Code Made By Myself

Board.cs {
    public static CustomInitial(String fenString)
    private void customAddPieces(String fenString)
}

## Markdown UML Diagram of the Chess Project

```mermaid
classDiagram
direction RL


class GameState {
    + board: Board
    + CurrentPlayer: Player
    + Result: Result = null
    + GameState(Player, Board): GameState
    + getBoard(): Board
    + getCurrentPlayer(): Player
    - setCurrentPlayer(Player): void
    + getResult(): Result
    - setResult(Result): void
    + LegalMovesForPiece(Position): IEnumerable&lt; Move &gt;
    + MakeMove(Move): void
    + AllLegalMovesFor(Player): IEnumerable&lt; Move &gt;
    + CheckForGameOver(): void
    + IsGameOver(): bool
}

class EndReason {
    <<Enumeration>>
    Checkmate
    Stalemate
    FiftyMoveRule
    InsufficientMaterial
    ThreefoldRepetition
}

class Result {
    + Winner: Player
    + EndReason: EndReason
    + Result(Player, EndReason): Result
    + getWinner(): Player
    + getEndReason(): EndReason
    + static Win(Player): Result
    + static Draw(EndReason): Result
}

class Player {
    <<Enumeration>>
    None
    White
    Black
    + static Opponent(Player): Player
}

class Board {
    - pieces: Piece[][]
    + this[int, int]: Piece
    + this[Position]: Piece
    + Board(): Board
    + static Initial(): Board
    - AddStatrtPieces(): void
    + static IsInside(Position): bool
    + IsEmpty(Position): bool
    + PiecePositions(): IEnumerable&lt; Position &gt;
    + PiecePositionsFor(Player): IEnumerable&lt; Position &gt;
    + IsInCheck(Player): bool
    + Copy(): Board
}

class Piece {
    <<Abstract>>
    + Type: PieceType
    + Color: Player
    + HasMoved: bool = false
    + getType(): PieceType
    + getColor(): Player
    + getHasMoved(): bool
    + setHasMoved(bool): void
    + Copy(): Piece
    + GetMoves(Position, Board): IEnumerable&lt Move &gt
    # MovePositionsInDir(Position, Board, Direction): IEnumerable&lt; Position &gt;
    # MovePositionsInDirs(Position, Board, Direction[]): IEnumerable&lt; Position &gt;
    + CanCaptureOpponentKing(Position, Board): bool
}

class Pawn {
    - forward: Direction
    + Pawn(Player): Pawn
    - static CanMoveTo(Position, Board): bool
    - CanCaptureAt(Position, Board): bool
    - ForwardMoves(Position, Board): IEnumerable&lt; Move &gt;
    - DiagonalMoves(Position, Board): IEnumerable&lt; Move &gt;
}
class Knight {
    + Knight(Player): Knight
    - static PotentialToPositions(Position): IEnumerable&lt; Position &gt;
    - MovePositions(Position, Board): IEnumerable&lt; Position &gt;
}
class Bishop {
    + static dirs: Direction[]
    + Bishop(Player): Bishop
}
class Rook {
    + static dirs: Direction[]
    + Rook(Player): Rook
}
class Queen {
    + static dirs: Direction[]
    + Queen(Player): Queen
}
class King {
    + static dirs: Direction[]
    + King(Player): King
    - MovePositions(Position, Board): IEnumerable&lt; Position &gt;
}

class Position {
    + Row: int
    + Column: int
    + Position(int, int): Position
    + getRow(): int
    + getColumn(): int
    + SquareColor(): Player
    + Equals(object): bool
    + GetHashCode(): int
    + operator==(Position, Position): bool
    + operator!=(Position, Position): bool
    + operator+(Position, Direction): Position
}

class Direction {
    + RowDelta: int
    + ColumnDelta: int
    + Direction(int, int): Direction
    + getRowDelta(): int
    + getColumnDelta(): int
    + operator+(Direction, Direction): Direction
    + operator*(int, Direction)    
}

class MoveType {
    <<Enumeration>>
    Normal
    CastleKS
    CastleQS
    DoublePawn
    EnPassant
    PawnPromotion
}

class Move {
    <<Abstract>>
    + Type: MoveType
    + FromPos: Position
    + ToPos: Position
    + getType(): MoveType
    + getFromPos(): Position
    + getToPos(): Position
    + Execute(Board): void
    + IsLegal(Board): bool
}

class NormalMove {
    + NormalMove(Position, Position): NormalMove
}

GameState <-- Board
GameState <-- Player
GameState <-- Result

Result <-- Player
Result <-- EndReason

Move <|.. NormalMove
Move <-- Position
Move <-- MoveType

Board *-- Piece

Piece <|.. Pawn
Piece <|.. Knight
Piece <|.. Bishop
Piece <|.. Rook
Piece <|.. Queen
Piece <|.. King
Piece <-- PieceType
Piece <-- Player


```