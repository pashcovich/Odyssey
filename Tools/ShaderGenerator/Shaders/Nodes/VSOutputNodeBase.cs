using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    [DataContract]
    public abstract class VSOutputNodeBase : NodeBase
    {
        [DataMember]
        [SupportedType(Type.Float4)]
        public INode Position { get; set; }
        
        [DataMember]
        [SupportedType(Type.Struct)]
        public override IVariable Output { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Position;
                foreach (var node in Position.DescendantNodes)
                    yield return node;
            }
        }

        public VSOutputNodeBase()
        {
            IsVerbose = true;
        }

        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<NullReferenceException>(Output != null);
            base.Validate(key);
        }

        public override string Operation()
        {
            Struct vsOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", vsOutput.Type, vsOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Position], Position.Access()));
            //sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.WorldPosition], WorldPosition.Access()));
            sb.AppendLine(string.Format("\treturn {0};", vsOutput.Name));

            return sb.ToString();
        }

        public override string Access()
        {
            return string.Empty;
        }
    }
}
