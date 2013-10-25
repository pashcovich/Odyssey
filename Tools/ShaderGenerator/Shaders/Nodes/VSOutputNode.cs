using Odyssey.Content.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    [DataContract]
    public abstract class VSOutputNode : VSOutputNodeBase
    {
        [DataMember]
        [SupportedType(Type.Float3)]
        public INode Normal { get; set; }

        [DataMember]
        [SupportedType(Type.Float2)]
        public INode TextureUV { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (var node in base.DescendantNodes)
                    yield return node;

                yield return Normal;
                foreach (var node in Normal.DescendantNodes)
                    yield return node;

                yield return TextureUV;
                foreach (var node in TextureUV.DescendantNodes)
                    yield return node;
            }
        }


    }
}
