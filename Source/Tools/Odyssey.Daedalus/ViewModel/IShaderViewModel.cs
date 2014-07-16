using Odyssey.Daedalus.Model;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.ViewModel
{
    public interface IShaderViewModel
    {
        string ColorizedSourceCode { get; }
        CompilationStatus CompilationStatus { get; set; }
        FeatureLevel FeatureLevel { get; }
        ShaderModel ShaderModel { get; }
        bool IsEmpty { get; }
        string Name { get; }
        string SourceCode { get; }
        System.Collections.ObjectModel.ObservableCollection<TechniqueMappingViewModel> Techniques { get; set; }
        ShaderType Type { get; }
        System.Windows.Input.ICommand ViewCodeCommand { get; }
    }
}
