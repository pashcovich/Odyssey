using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    [YamlMapping(typeof(YamlClipSpaceTransformNode))]
    public class ClipSpaceTransformNode : NodeBase
    {
        IVariable output;
        ClipSpaceTransform clipSpaceMethod;

        [SupportedType(Type.Float3)]
        public INode Position { get; set; }
        [SupportedType(Type.Float3)]
        public IVariable InstancePosition { get; set; }
        [SupportedType(Type.Float)]
        public IVariable Size { get; set; }
        [SupportedType(Type.Float2)]
        public IVariable ScreenSize{ get; set; }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    string id = string.Empty;
                    Output = new Vector
                    {
                        Name = "vResult",
                        Type = Type.Float4
                    };
                }
                return output;
            }
            set { output = value; }
        }

        public override IEnumerable<IMethod> RequiredMethods { get { yield return clipSpaceMethod; } }

        public override IEnumerable<INode> DescendantNodes
        {
            get { yield return Position; }
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            clipSpaceMethod = new ClipSpaceTransform();
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.FullName, Access());
        }

        public override string Access()
        {
            return clipSpaceMethod.Call((Vector)Position.Output, (Vector)InstancePosition, (Vector)Size, (Vector)ScreenSize);
        }

    }
}
