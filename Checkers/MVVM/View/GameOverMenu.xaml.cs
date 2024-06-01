using Checkers.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Checkers.MVVM
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<MenuOption> OptionSelected;
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();
            Result result=gameState.Result;
            SerializeStats(result.Winner, gameState);
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(gameState.CurrentPlayer);
        }

        private void SerializeStats(Player winner, GameState gameState)
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string directoryPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "MVVM\\View\\Resources"));

            List<int> stats = WelcomeMenuWindow.DeserializeStats();

            if (winner == Player.White)
                stats[0] = stats[0] + 1;
            else
                stats[1] = stats[1] + 1;

            int maxPiecesLeftEver = stats[2];
            int maxPiecesLeftLocal = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (gameState.Board[row, column] != null)
                        maxPiecesLeftLocal++;
                }
            }

            if (maxPiecesLeftLocal < maxPiecesLeftEver)
                maxPiecesLeftEver = maxPiecesLeftLocal;

            stats[2] = maxPiecesLeftEver;


            string filePath = Path.Combine(directoryPath, "Stats.json");
            string jsonString = JsonSerializer.Serialize<List<int>>(stats);
            File.WriteAllText(filePath, jsonString);
        }

        public static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "Wite Player Won",
                Player.Black => "Black Player Won",
                _ => ""
            };
        }

        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "White",
                Player.Black => "Black",
                _ => ""
            };
        }

        private static string GetReasonText(Player currentPlayer)
        {
            return currentPlayer switch
            {
                Player.White => $"{PlayerString(currentPlayer)} lost all its pieces",
                Player.Black => $"{PlayerString(currentPlayer)} lost all its pieces",
                _ => ""
            };
        }

        private void Restart_Clicked(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Restart);
        }
        private void Exit_Clicked(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Exit);
        }
    }
}
