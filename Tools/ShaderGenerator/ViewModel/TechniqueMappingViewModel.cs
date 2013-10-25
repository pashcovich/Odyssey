using GalaSoft.MvvmLight;
using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class TechniqueMappingViewModel : ViewModelBase
    {
        int index;
        public TechniqueMapping TechniqueMapping { get; internal set; }
        public int Index { get { return index; } set { index = value; RaisePropertyChanged("Index"); } }
        public string Name { get { return TechniqueMapping.Name; } }

        public void CreateKey(IEnumerable<ShaderDescriptionViewModel> shaders)
        {
            var vertexShader = shaders.First(s => s.Type == ShaderType.Vertex);
            var pixelShader = shaders.First(s=> s.Type == ShaderType.Pixel);
            VertexShaderFlags vsFlags = vertexShader != null ? (VertexShaderFlags)vertexShader.KeyPart : VertexShaderFlags.None;
            PixelShaderFlags psFlags = pixelShader != null ? (PixelShaderFlags)pixelShader.KeyPart : PixelShaderFlags.None;
            TechniqueKey tKey = new TechniqueKey(vsFlags, psFlags);
            TechniqueMapping.Key = tKey;
        }

        public bool ContainsShader(string shaderName)
        {
            return TechniqueMapping.Contains(shaderName);
        }
    }
}
