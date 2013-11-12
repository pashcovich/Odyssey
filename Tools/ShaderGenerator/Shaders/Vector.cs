using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public enum Swizzle
    {
        Null = 0,
        X = 1,
        Y = 2,
        Z = 3,
        W = 4
    }

    [YamlMapping(typeof(YamlVector))]
    [DataContract]
    public partial class Vector : Variable
    {
        private Swizzle[] swizzle;

        [DataMember]
        public float[] Value { get; set; }

        [DataMember]
        public Swizzle[] Swizzle
        {
            get { return swizzle; }
            set
            {
                Contract.Requires<ArgumentNullException>(value!= null);
                Contract.Requires<ArgumentException>(value.Length == ComponentsFromType(Type));
                swizzle = value;
            }
        }

        [DataMember]
        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public override Type Type
        {
            get
            {
                return base.Type;
            }
            set
            {
                var attributes = this.GetType().GetProperty("Type").GetCustomAttributes(true).OfType<SupportedTypeAttribute>();
                if (!attributes.Any(att => att.SupportedType == value))
                    throw new InvalidOperationException("Cannot assign a non-Vector type to this variable.");
                base.Type = value;
                
                
            }
        }

        public bool HasSwizzle { get { return swizzle != null; } }

        public Vector()
        {
        }

        public string PrintSwizzle()
        {
            string result = string.Empty;
            foreach (Swizzle s in Swizzle)
            {
                if (s == Shaders.Swizzle.Null)
                    continue;
                result += s.ToString().ToLower();
            }
            return result;
        }

        public static int ComponentsFromType(Shaders.Type type)
        {
            int components = 0;
            switch (type)
            {
                case Shaders.Type.Float:
                    components = 1;
                    break;
                case Shaders.Type.Float2:
                    components = 2;
                    break;

                case Shaders.Type.Float3:
                    components = 3;
                    break;

                case Shaders.Type.Float4:
                    components = 4;
                    break;
            }

            return components;
        }

    }
}
