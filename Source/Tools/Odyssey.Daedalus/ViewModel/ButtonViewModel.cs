using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Input;

namespace Odyssey.Daedalus.ViewModel
{
    public class ButtonViewModel : ViewModelBase
    {
        string tooltip;
        UIElement content;
        private CommandViewModel mainCommand;
        private List<CommandViewModel> secondaryCommands;
        private Key key;

        public UIElement Content { get { return content; } set { content = value; RaisePropertyChanged("Content"); } }
        public bool HasOtherCommands { get { return secondaryCommands != null && secondaryCommands.Any(); }}

        public CommandViewModel MainCommand { get { return mainCommand; } set { mainCommand = value; RaisePropertyChanged("MainCommand"); } }
        public IEnumerable<CommandViewModel> SecondaryCommands { get { return secondaryCommands; } internal set
        {
            secondaryCommands = new List<CommandViewModel>(value); RaisePropertyChanged("SecondaryCommands");
        }}

        public string Tooltip
        {
            get { return tooltip; }
            set
            {
                tooltip = value;
                RaisePropertyChanged("Tooltip");
            }
        }

        public Key Key
        {
            get { return key; }
            set
            {
                key = value;
                RaisePropertyChanged("Key");
            }
        }


    }
}
