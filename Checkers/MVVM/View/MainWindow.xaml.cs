using Checkers.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        private SaveMenu saveMenu;

        private GameState gameState;
        private Position selectedPosition = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            gameState = new GameState(Board.Initial(), Player.White, WelcomeMenuWindow.MultipleJumpValidator());
            DrawBoard(gameState.Board);
        }

        public MainWindow(GameState gameState)
        {
            InitializeComponent();
            InitializeBoard();

            this.gameState = gameState;
            DrawBoard(gameState.Board);
        }

        private void InitializeBoard()
        {
            for(int row=0;row<8;row++)
            {
                for(int column=0;column<8;column++)
                {
                    Image image = new Image();
                    pieceImages[row,column] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle higlight= new Rectangle();
                    highlights[row,column] = higlight;
                    HighlightGrid.Children.Add(higlight);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Piece piece = board[row, column];
                    pieceImages[row, column].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(IsMenuOnScreen())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position position = ToSquarePosition(point);

            if(selectedPosition == null)
                OnFromPositionSelected(position);
            else
                OnToPositionSelected(position);
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y/squareSize);
            int column = (int)(point.X / squareSize);

            return new Position(row, column);
        }

        private void OnFromPositionSelected(Position position)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(position);

            if(moves.Any())
            {
                selectedPosition = position;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private void OnToPositionSelected(Position position)
        {
            selectedPosition = null;
            HideHighlights();

            if(moveCache.TryGetValue(position, out Move move))
            {
                HandleMove(move);
            }
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);

            if(gameState.IsGameOver())
                ShowGamOver();
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach (Move move in moves)
            {
                moveCache[move.ToPosition]= move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach(Position toPosition in moveCache.Keys)
            {
                highlights[toPosition.Row, toPosition.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach(Position toPosition in moveCache.Keys)
            {
                highlights[toPosition.Row, toPosition.Column].Fill = Brushes.Transparent;
            }
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGamOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == MenuOption.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    ExitGame();
                }
            };
        }

        private void ExitGame()
        {
            Application.Current.Shutdown();
        }

        private void RestartGame()
        {
            selectedPosition = null;
            HideHighlights();
            moveCache.Clear();
            gameState = new GameState(Board.Initial(), Player.White, WelcomeMenuWindow.MultipleJumpValidator());
            DrawBoard(gameState.Board);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(!IsMenuOnScreen() && e.Key == Key.Escape)
            {
                ShowPauseAndSaveMenu();
            }
        }

        private void ShowPauseAndSaveMenu()
        {
            PauseAndSaveMenu pauseAndSave = new PauseAndSaveMenu();
            MenuContainer.Content = pauseAndSave;

            pauseAndSave.OptionSelected += option =>
            {
                MenuContainer.Content = null;

                if (option == MenuOption.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                if (option == MenuOption.Exit)
                    ExitGame();
                if (option == MenuOption.SaveMenu)
                    ShowSaveMenu();
            };
        }

        private void ShowSaveMenu()
        {
            saveMenu = new SaveMenu();
            MenuContainer.Content = saveMenu;

            saveMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;

                if (option == MenuOption.Save)
                {
                    Save();
                }
                if(option == MenuOption.Continue)
                    ShowPauseAndSaveMenu();
            };
        }
        //private void Save()
        //{
        //    List<GameStateToSerialize> games = new List<GameStateToSerialize>();
        //    System.Tuple<int, int, int>[] tuple = new System.Tuple<int, int, int>[64];

        //    GameStateToSerialize game = new GameStateToSerialize("First", null, 0, 0);
        //    game.SerializeGame(game, games);
        //    games = game.DeserializeGames();
        //    ShowPauseAndSaveMenu();
        //}

        private void Save()
        {
            List<GameStateToSerialize> games = new List<GameStateToSerialize>();
            string saveName = saveMenu.SaveName;

            GameStateToSerialize gameStateToSerialize = gameState.PrepareSerialization(saveName);
            games = gameStateToSerialize.DeserializeGames();
            gameStateToSerialize.SerializeGame(gameStateToSerialize, games);
            ShowPauseAndSaveMenu();
        }
    }
}
