using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Materials
{
    [DataContract]
    [DebuggerDisplay("{Name}")]
    public class TechniqueMapping
    {
        [DataMember]
        Dictionary<ShaderType, ShaderDescription> techniqueMap;

        [DataMember]
        public TechniqueKey Key { get; internal set; }

        [DataMember]
        public string Name { get; set; }

        public IEnumerable<ShaderDescription> Shaders { get { return techniqueMap.Values; } }

        public TechniqueMapping(string name)
        {
            techniqueMap = new Dictionary<ShaderType, ShaderDescription>();
            Name = name;
        }

        public void Set(ShaderDescription shader)
        {
            techniqueMap[shader.ShaderType] = shader;
        }

        public void Remove(ShaderType shaderType)
        {
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

        public bool TryGetValue(ShaderType type, out ShaderDescription shader)
        {
            shader = null; 
            if (techniqueMap.ContainsKey(type))
            {
                shader = techniqueMap[type];
                return true;
            }
            return false;
        }

        public bool Validate()
        {
            return Shaders.Aggregate(true, (current, shaderDesc) => current & shaderDesc.Validate());
        }


        public ShaderDescription this[ShaderType type]
        {
            get
            {
                return techniqueMap[type];
            }
        }

        
    }
}