using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

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
