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

namespace Checkers.MVVM.View
{
    /// <summary>
    /// Interaction logic for AboutMenu.xaml
    /// </summary>
    public partial class AboutMenu : UserControl
    {
        public AboutMenu()
        {
            InitializeComponent();
        }

        private void Back_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
