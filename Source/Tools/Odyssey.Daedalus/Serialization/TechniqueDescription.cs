using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using SharpDX.Serialization;
using Shader = Odyssey.Tools.ShaderGenerator.Shaders.Shader;

namespace Odyssey.Tools.ShaderGenerator.Serialization
{
    public class TechniqueDescription : IDataSerializable
    {
        private string name;
        private TechniqueKey key;
        private Dictionary<ShaderType, Shader> shaders;

        public string Name { get { return name; } }
        public TechniqueKey Key { get { return key; } }

        public IEnumerable<Shader> Shaders
        {
            get { return shaders.Values; }
        }

        public TechniqueDescription()
        {
            shaders = new Dictionary<ShaderType, Shader>();
        }

        public TechniqueDescription(string name, TechniqueKey key, IEnumerable<Shader> shaders)
        {
            this.name = name;
            this.key = key;
            this.shaders = new Dictionary<ShaderType, Shader>(shaders.ToDictionary(s => s.Type, s=>s));
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.BeginChunk("TECH");
            serializer.Serialize(ref name);
            serializer.Serialize(ref key);

            var shaderList = shaders.Values.ToList();
            serializer.Serialize(ref shaderList);
            serializer.EndChunk();

            if (serializer.Mode == SerializerMode.Read)
                shaders = shaderList.ToDictionary(s => s.Type, s=>s);
        }
    }
}
