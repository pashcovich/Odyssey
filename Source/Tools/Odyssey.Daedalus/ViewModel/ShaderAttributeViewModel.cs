using GalaSoft.MvvmLight;
using Odyssey.Graphics.Effects;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class ShaderAttributeViewModel : ViewModelBase
    {
        string name;
        bool selected;
        
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                RaisePropertyChanged("Selected");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public ShaderType Type {get; internal set;}


    }
}
