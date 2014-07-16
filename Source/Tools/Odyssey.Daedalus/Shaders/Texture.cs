using Odyssey.Daedalus.Shaders.Nodes;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Odyssey.Daedalus.Shaders
{
    [DataContract]
    public partial class Texture : Variable
    {
        [DataMember]
        [SupportedType(Type.Texture2D)]
        [SupportedType(Type.Texture3D)]
        [SupportedType(Type.TextureCube)]
        public override Type Type
        {
            get
            {
                return base.Type;
            }
            set
            {
                var attributes = this.GetType().GetProperty("Type").GetCustomAttributes(true).OfType<SupportedTypeAttribute>();
                if (attributes.All(att => att.SupportedType != value))
                    throw new InvalidOperationException("Cannot assign a non-Texture type to this variable.");
                base.Type = value;
            }
        }
    }
}