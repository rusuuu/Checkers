using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
using Path = System.IO.Path;

namespace Checkers.MVVM
{
    /// <summary>
    /// Interaction logic for OptionsMenu.xaml
    /// </summary>
    public partial class OptionsMenu : UserControl
    {
        public OptionsMenu()
        {
            InitializeComponent();
            if (WelcomeMenuWindow.MultipleJumpValidator() == true)
                MultipleJumpBox.IsChecked = true;
            else
                MultipleJumpBox.IsChecked = false;
        }

        private void Selected(object sender, RoutedEventArgs e)
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string directoryPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "MVVM\\View\\Resources"));

            int validator;

            if (MultipleJumpBox.IsChecked == true)
            {
                validator = 1;
            }
            else
            {
                validator = 0;
            }

            string filePath = Path.Combine(directoryPath, "MultipleJump.json");
            string jsonString = JsonSerializer.Serialize<int>(validator);
            File.WriteAllText(filePath, jsonString);
        }

        private void Back_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
