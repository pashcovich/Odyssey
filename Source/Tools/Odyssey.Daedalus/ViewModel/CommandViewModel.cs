using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace Odyssey.Daedalus.ViewModel
{
    public class CommandViewModel : ViewModelBase
    {
        private string label;
        private ICommand command;

        public string Label { get { return label; } set { label = value; RaisePropertyChanged("TextBlock"); } }
        public ICommand Command { get { return command; } set { command= value; RaisePropertyChanged("Command"); } }
    }
}
