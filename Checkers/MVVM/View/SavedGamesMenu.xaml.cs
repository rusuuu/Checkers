using System.Windows.Controls;

namespace Checkers.MVVM
{
    /// <summary>
    /// Interaction logic for SavedGamesMenu.xaml
    /// </summary>
    public partial class SavedGamesMenu : UserControl
    {
        public SavedGamesMenu()
        {
            InitializeComponent();
            GameStateToSerialize gameStateToSerialize = new GameStateToSerialize();
            SavedGamesList.ItemsSource = gameStateToSerialize.DeserializeGames();
        }

        private void Item_Selected(object sender, SelectionChangedEventArgs e)
        {
            GameStateToSerialize selectedItem = (GameStateToSerialize)SavedGamesList.SelectedItem;
            GameState gameState = new GameState();
            gameState.PrepareDeserialization(selectedItem);
            MainWindow newGame = new MainWindow(gameState);
            newGame.Show();
        }

        private void Back_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility=System.Windows.Visibility.Collapsed;
        }
    }
}
