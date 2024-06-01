using Checkers.MVVM;
using Checkers.MVVM.View;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for WelcomeMenuWindow.xaml
    /// </summary>
    public partial class WelcomeMenuWindow : Window
    {
        private SavedGamesMenu savedGamesMenuInstance;
        private OptionsMenu optionsMenuInstance;
        private AboutMenu aboutMenuInstance;
        private StatsMenu statsMenuInstance;

        public WelcomeMenuWindow()
        {
            InitializeComponent();
        }

        private void ContinueGame_Clicked(object sender, RoutedEventArgs e)
        {
            savedGamesMenuInstance ??= new SavedGamesMenu();
            MenuContainer.Content = savedGamesMenuInstance;
            if (savedGamesMenuInstance.Visibility != Visibility.Visible)
                savedGamesMenuInstance.Visibility = Visibility.Visible;
        }

        private void Options_Clicked(object sender, RoutedEventArgs e)
        {
            optionsMenuInstance ??= new OptionsMenu();
            MenuContainer.Content = optionsMenuInstance;
            if(optionsMenuInstance.Visibility != Visibility.Visible)
                optionsMenuInstance.Visibility = Visibility.Visible;
        }

        private void About_Clicked(object sender, RoutedEventArgs e)
        {
            aboutMenuInstance ??= new AboutMenu();
            MenuContainer.Content = aboutMenuInstance;
            if(aboutMenuInstance.Visibility != Visibility.Visible)
                aboutMenuInstance.Visibility = Visibility.Visible;
        }

        private void Stats_Clicked(object sender, RoutedEventArgs e)
        {
            statsMenuInstance ??= new StatsMenu();
            MenuContainer.Content = statsMenuInstance;
            if(statsMenuInstance.Visibility != Visibility.Visible)
                statsMenuInstance.Visibility = Visibility.Visible;
        }


        public static bool MultipleJumpValidator()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "MVVM\\View\\Resources\\MultipleJump.json"));

            if (!File.Exists(filePath))
            {
                return false;
            }

            string jsonContent = File.ReadAllText(filePath);
            int validator = JsonSerializer.Deserialize<int>(jsonContent);

            if (validator == 1)
                return true;
            else
                return false;
        }

        public static List<int> DeserializeStats()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "MVVM\\View\\Resources\\Stats.json"));

            if (!File.Exists(filePath))
            {
                return new List<int>();
            }

            string jsonContent = File.ReadAllText(filePath);
           return JsonSerializer.Deserialize<List<int>>(jsonContent);            
        }
    }
}
