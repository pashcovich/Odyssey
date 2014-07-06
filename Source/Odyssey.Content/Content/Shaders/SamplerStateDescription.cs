using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Materials
{
    [DataContract]
    public class SamplerStateDescription
    {
        [DataMember]
        public int Index { get; internal set; }
        [DataMember]
        public Filter Filter { get; internal set; }
        [DataMember]
        public TextureAddressMode TextureAddressMode { get; internal set; }
        [DataMember]
        public Comparison Comparison { get; internal set;}
    }
}
