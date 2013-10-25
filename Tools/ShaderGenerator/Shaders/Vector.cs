using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    [DataContract]
    public partial class Vector : Variable
    {

        [DataMember]
        public float? Value { get; set; }

        [DataMember]
        public Swizzle[] Swizzle { get; set;}

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

        public Vector()
        {
            Index = -1;
        }

        public string PrintSwizzle()
        {
            string result = string.Empty;
            foreach (Swizzle s in Swizzle)
                result += s.ToString().ToLower();
            return result;
        }
    }
}
