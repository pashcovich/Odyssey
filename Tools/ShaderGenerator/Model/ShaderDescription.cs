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
        public FeatureLevel FeatureLevel { get { return Shader.FeatureLevel; } set { Shader.FeatureLevel = value; } }
        public CompilationStatus CompilationStatus { get; set; }
        public ShaderType Type { get { return Shader.Type; } }
        public TechniqueKey KeyPart { get { return Shader.KeyPart; } }

        public string SourceCode { get { return Shader.SourceCode; } }
    }
}
