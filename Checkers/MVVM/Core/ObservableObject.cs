using Checkers.MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Checkers.MVVM
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
