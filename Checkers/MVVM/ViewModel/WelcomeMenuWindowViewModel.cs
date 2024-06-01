using Checkers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.MVVM
{
    public class WelcomeMenuWindowViewModel: ObservableObject
    {
        public WelcomeMenuWindowViewModel()
        {
            
        }

        private ICommand openNewGameCommand;
        public ICommand OpenNewGameCommand
        {
            get
            {
                if (openNewGameCommand == null)
                {
                    openNewGameCommand = new RelayCommand(OpenNewGameWindow);
                }
                return openNewGameCommand;
            }
        }
        

        private void OpenNewGameWindow(object commandParameter)
        {
            MainWindow newGame = new MainWindow();
            newGame.Show();
        }
    }
}
