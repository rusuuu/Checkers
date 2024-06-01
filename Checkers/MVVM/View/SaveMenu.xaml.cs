using System;
using System.Windows.Controls;

namespace Checkers.MVVM
{
    /// <summary>
    /// Interaction logic for SaveMenu.xaml
    /// </summary>
    public partial class SaveMenu : UserControl
    {
        public string SaveName { get; set; }

        public event Action<MenuOption> OptionSelected;
        public SaveMenu()
        {
            InitializeComponent();
        }

        private void Continue_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            OptionSelected?.Invoke(MenuOption.Continue);
        }

        private void Save_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveName = UserInputTextBox.Text;
            OptionSelected?.Invoke(MenuOption.Save);
        }

        private void PlaceHolderTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            PlaceHolderTextBox.Visibility = System.Windows.Visibility.Collapsed;
            UserInputTextBox.Visibility = System.Windows.Visibility.Visible;
            UserInputTextBox.Focus();
        }

        private void UserInputTextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(UserInputTextBox.Text))
            {
                UserInputTextBox.Visibility = System.Windows.Visibility.Collapsed;
                PlaceHolderTextBox.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
