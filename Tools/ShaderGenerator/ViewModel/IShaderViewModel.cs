using System;
namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public interface IShaderViewModel
    {
        string ColorizedSourceCode { get; }
        Odyssey.Tools.ShaderGenerator.Model.CompilationStatus CompilationStatus { get; set; }
        Odyssey.Graphics.Materials.FeatureLevel FeatureLevel { get; }
        Odyssey.Graphics.Materials.ShaderModel ShaderModel { get; }
        bool IsEmpty { get; }
        string Name { get; }
        string SourceCode { get; }
        System.Collections.ObjectModel.ObservableCollection<TechniqueMappingViewModel> Techniques { get; set; }
        Odyssey.Graphics.Rendering.ShaderType Type { get; }
        System.Windows.Input.ICommand ViewCodeCommand { get; }
    }
}
