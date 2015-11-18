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

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode(nameof(Normal), Normal);
        }
        
    }
}
