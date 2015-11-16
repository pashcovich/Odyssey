using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Daedalus.Model
{
    public class ShaderDescription
    {
        public Shaders.Shader Shader { get; internal set; }
        public string Name { get { return Shader.Name; } }
        public FeatureLevel FeatureLevel { get { return Shader.FeatureLevel; } set { Shader.FeatureLevel = value; } }
        public CompilationStatus CompilationStatus { get; set; }
        public ShaderType Type { get { return Shader.Type; } }
        public TechniqueKey KeyPart { get { return Shader.KeyPart; } }

        public string SourceCode { get { return Shader.SourceCode; } }
    }
}
