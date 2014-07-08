using Odyssey.Graphics.Effects;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public interface IShaderViewModel
    {
        string ColorizedSourceCode { get; }
        Odyssey.Tools.ShaderGenerator.Model.CompilationStatus CompilationStatus { get; set; }
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
