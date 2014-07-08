using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class CommandViewModel : ViewModelBase
    {
        private string label;
        private ICommand command;

        public string Label { get { return label; } set { label = value; RaisePropertyChanged("Label"); } }
        public ICommand Command { get { return command; } set { command= value; RaisePropertyChanged("Command"); } }
    }
}
