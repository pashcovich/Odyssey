using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content.Shaders
{
    [DataContract]
    public class TechniqueMapping
    {
        [DataMember]
        Dictionary<ShaderType, ShaderObject> techniqueMap;

        [DataMember]
        public TechniqueKey Key { get; internal set; }

        [DataMember]
        public string Name { get; private set; }

        public IEnumerable<ShaderObject> Shaders { get { return techniqueMap.Values; } }

        public TechniqueMapping(string name)
        {
            techniqueMap = new Dictionary<ShaderType, ShaderObject>();
            Name = name;
        }

        public void Set(ShaderObject shader)
        {
            Contract.Requires<ArgumentNullException>(shader != null);
            techniqueMap[shader.ShaderType] = shader;
        }

        public void Remove(ShaderType shaderType)
        {
            Contract.Requires<ArgumentException>(Contains(shaderType));
            techniqueMap.Remove(shaderType);
        }

        public bool Contains(ShaderType type)
        {
            return techniqueMap.ContainsKey(type);
        }

        public bool Contains(string shaderName)
        {
            return techniqueMap.Values.Count(s => s.Name == shaderName) > 0;
        }

        public bool TryGetValue(ShaderType type, out ShaderObject shader)
        {
            shader = null; 
            if (techniqueMap.ContainsKey(type))
            {
                shader = techniqueMap[type];
                return true;
            }
            return false;
        }

        public ShaderObject this[ShaderType type]
        {
            get
            {
                Contract.Requires<KeyNotFoundException>(Contains(type));
                return techniqueMap[type];
            }
        }
    }
}