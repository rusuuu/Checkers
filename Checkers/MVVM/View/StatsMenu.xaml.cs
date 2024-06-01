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
    /// Interaction logic for StatsMenu.xaml
    /// </summary>
    public partial class StatsMenu : UserControl
    {
        public StatsMenu()
        {
            InitializeComponent();
            DisplayPlayersPoints();
        }

        public void DisplayPlayersPoints()
        {
            List<int> points = WelcomeMenuWindow.DeserializeStats();

            int whitePoints = 0;
            int blackPoints = 0;
            int piecesLeft = 0;
            if (points.Count > 0 ) 
            {
                whitePoints = points[0];
                blackPoints = points[1];
                piecesLeft = points[2];
            }
            
            WhitePoints.Text = $"{whitePoints} Points";
            BlackPoints.Text = $"{blackPoints} Points";
            PiecesLeft.Text = $"{piecesLeft} Pieces";
        }

        private void Back_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
