using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class ButtonViewModel : ViewModelBase
    {
        string label;
        string tooltip;
        UIElement content;
        ICommand command;
        public UIElement Content { get { return content; } set { content = value; RaisePropertyChanged("Content"); } }
        public ICommand Command { get { return command; } set { command = value; RaisePropertyChanged("Command"); } }

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                RaisePropertyChanged("Label");
            }
        }

        public string Tooltip
        {
            get { return tooltip; }
            set
            {
                tooltip = value;
                RaisePropertyChanged("Tooltip");
            }
        }


    }
}
