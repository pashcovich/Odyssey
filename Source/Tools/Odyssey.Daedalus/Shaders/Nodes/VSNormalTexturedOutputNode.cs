using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes
{
    [DataContract]
    public class VSNormalTexturedOutputNode : VSTexturedOutputNode
    {
        [DataMember]
        [SupportedType(Type.Float3)]
        [IgnoreValidation(true)]
        public INode Normal { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (var node in base.DescendantNodes)
                    yield return node;

                if (Normal != null)
                {
                    yield return Normal;
                    foreach (var node in Normal.DescendantNodes)
                        yield return node;
                }
            }
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode("Normal", Normal);
        }
        
    }
}
