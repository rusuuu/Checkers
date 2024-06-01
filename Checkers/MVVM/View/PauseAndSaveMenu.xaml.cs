using System;
using System.Windows;
using System.Windows.Controls;

namespace Checkers.MVVM
{
    /// <summary>
    /// Interaction logic for PauseAndSaveMenu.xaml
    /// </summary>
    public partial class PauseAndSaveMenu : UserControl
    {

        public event Action<MenuOption> OptionSelected;
        public PauseAndSaveMenu()
        {
            InitializeComponent();
        }

        private void Continue_Clicked(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Continue);
        }

        private void Restart_Clicked(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Restart);
        }

        private void Exit_Clicked(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Exit);
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.SaveMenu);
        }
    }
}
