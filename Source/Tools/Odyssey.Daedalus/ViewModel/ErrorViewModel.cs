using GalaSoft.MvvmLight;
using Odyssey.Daedalus.Model;

namespace Odyssey.Daedalus.ViewModel
{
    public class ErrorViewModel : ViewModelBase
    {
        public ErrorModel ErrorModel { get; set; }

        public int Line { get { return ErrorModel.Line; } }
        public int Column { get { return ErrorModel.Column; } }
        public string Shader { get { return ErrorModel.Shader; } }
        public string Description { get { return ErrorModel.Description; } }
        public Category Category { get { return ErrorModel.Category; } } 
    }
}
