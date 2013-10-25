using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Model
{
    public class ShaderDescription
    {
        public Shader Shader { get; internal set; }
        public string Name { get { return Shader.Name; } }
        public FeatureLevel FeatureLevel { get { return Shader.FeatureLevel; } }
        public CompilationStatus CompilationStatus { get; set; }
        public ShaderType Type { get { return Shader.Type; } }

        public string SourceCode { get { return Shader.SourceCode; } }
    }
}
