using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Serialization;
using Shader = Odyssey.Daedalus.Shaders.Shader;

namespace Odyssey.Daedalus.Serialization
{
    public class TechniqueDescription : IDataSerializable
    {
        private string name;
        private TechniqueKey key;
        private Dictionary<ShaderType, Shaders.Shader> shaders;

        public string Name { get { return name; } }
        public TechniqueKey Key { get { return key; } }

        public IEnumerable<Shaders.Shader> Shaders
        {
            get { return shaders.Values; }
        }

        public TechniqueDescription()
        {
            shaders = new Dictionary<ShaderType, Shaders.Shader>();
        }

        public TechniqueDescription(string name, TechniqueKey key, IEnumerable<Shaders.Shader> shaders)
        {
            this.name = name;
            this.key = key;
            this.shaders = new Dictionary<ShaderType, Shaders.Shader>(shaders.ToDictionary(s => s.Type, s=>s));
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.BeginChunk("TECH");
            serializer.Serialize(ref name);
            serializer.Serialize(ref key);

            serializer.Serialize(ref shaders, serializer.SerializeEnum, (ref Shaders.Shader s) => serializer.Serialize(ref s));
            serializer.EndChunk();
        }
    }
}
