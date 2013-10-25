using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class DebugViewModel : ViewModelBase
    {

        ObservableCollection<ErrorViewModel> errors;
        public ObservableCollection<ErrorViewModel> Errors
        {
            get { return errors; }
            set
            {
                errors = value;
                RaisePropertyChanged("Errors");
                RaisePropertyChanged("HasErrors");
            }
        }

        public bool HasErrors
        {
            get {
                if (errors != null)
                    return errors.Count > 0;
                else return false;
            }
        }
    
    }
}
