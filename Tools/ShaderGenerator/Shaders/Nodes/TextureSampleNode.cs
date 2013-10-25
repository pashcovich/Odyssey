using Odyssey.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    public class TextureSampleNode : NodeBase
    {
        [SupportedType(Type.Texture2D)]
        public IVariable Texture { get; set; }

        [SupportedType(Type.Sampler)]
        [SupportedType(Type.SamplerComparisonState)]
        public IVariable Sampler { get; set; }

        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public INode Coordinates { get; set; }

        [SupportedType(Type.Float4)]
        public override IVariable Output { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Coordinates;
                foreach (var node in Coordinates.DescendantNodes)
                    yield return Coordinates;
            }
        }

        public TextureSampleNode()
        {
            Output = new Vector { Name = "cTexture", Type = Shaders.Type.Float4 };
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            Texture.SetMarkup(Shaders.Texture.SamplerIndex, Sampler.Index);
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        public override string Access()
        {
            bool requiresSwizzle = false;
            if (Coordinates.Output.Type == Type.Float3 || Coordinates.Output.Type == Type.Float4 && ((Vector)Coordinates.Output).Swizzle.Length == 2)
                requiresSwizzle = true;

            return string.Format("{0}.Sample({1}, {2})", Texture.Name, Sampler.Name,
                requiresSwizzle ? string.Format("{0}.{1}", Coordinates.Reference, ((Vector)Coordinates.Output).PrintSwizzle()) : Coordinates.Reference);
        }
    }
}
