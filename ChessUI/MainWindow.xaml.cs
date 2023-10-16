using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessRule;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        // We need a dictionary called move cache
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        // Key is toPosition, value is the move that places Position from to Position to
        // Show them on screen with highlights. Then when one of them is clicked, we use the dictionary to find
        // the move and execute it!

        private GameState gameState;
        private Position selectedPos = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            gameState = new GameState(Player.White, Board.Initial());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }

        public void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image image = new Image();      // For each, create an image control
                    pieceImages[r, c] = image;      // Store it on our 2d array
                    PieceGrid.Children.Add(image);  // Add it as a child to the UniformGrid PieceGrid in XAML

                    // We do the same thing for highlights
                    Rectangle highlight = new Rectangle();  // For each, create a rectangle
                    highlights[r, c] = highlight;           // Store it on the 2d array
                    HighlightGrid.Children.Add(highlight);  // Add it as child of HighlightGrid

                }
            }
        }

        // Draws Board using Images class, sets source of all image controls
        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Handle some user input!
            // Called when player clicks somewhere on the board

            // GetPosition provides pixelposition from top-left corner of window of where the click happened
            Point point = e.GetPosition(BoardGrid);     // Gets position of where the click happened
            // Use helper ToSquarePosition() method
            Position pos = ToSquarePosition(point);
            // What we do with this position depends on what was clicked (if empty, if piece, and then if curplayer piece)

            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        // Helper method to map window position to square position
        private Position ToSquarePosition(Point point)
        {
            // How large a square is with respect to window size?
            // Because board grid is always a square, we can just divide width and height of baord by 8
            double squareSize = BoardGrid.ActualWidth / 8;  
            // Found square size, now can measure point values by squares in board
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize); 

            return new Position(row, col);
        }

        // This method is called when a square is clicked but no currently selected piece
        // The "From" square is clicked
        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);
            // Return Empty if empty square, opponent piece, or piece that cannot move (no moves)

            if (moves.Any())    // If at least there's one move
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        // Method called when square is clicked with a selected piece
        // The "To" square is clicked
        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if (moveCache.TryGetValue(pos, out Move move))  // Try to get the move with toPos == pos (ClickPosition)
            {
                // If found/exists
                HandleMove(move);
            }
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }

        // Takes the legal moves of the selected piece and places it in cache
        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();      // First clear/empty moves in cache already
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;   // Add to dictionary with ToPos as key and move as value
            }

        }

        // Shows highlight for a square
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                // Color the rectangle by initializing the Fill member of Rectangle!
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        



    }
}
