using Odyssey.Graphics.Shaders;
using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public class TextureSampleNode : NodeBase
    {
        private IVariable output;

        [SupportedType(Type.Texture2D)]
        [SupportedType(Type.Texture3D)]
        [SupportedType(Type.TextureCube)]
        public IVariable Texture { get; set; }

        [SupportedType(Type.Sampler)]
        [SupportedType(Type.SamplerComparisonState)]
        public IVariable Sampler { get; set; }

        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        public INode Coordinates { get; set; }

        [SupportedType(Type.Float4)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    string name1 = Texture.Name.Substring(1, Texture.Name.Length - 1);

                    const Type type = Type.Float4;
                    Output = new Vector
                    {
                        Name = string.Format("c{0}", name1),
                        Type = type
                    };
                }
                return output;
            }
            set { output = value; }
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            Texture.SetMarkup(Shaders.Texture.SamplerIndex, Sampler.Index.ToString());
        }

        public override string Access()
        {
            return string.Format("{0}.Sample({1}, {2})", Texture.Name, Sampler.Name, Coordinates.Reference);
        }

        protected override void RegisterNodes()
        {
            AddNode("Coordinates", Coordinates);
        }

        protected override void SerializeVariables(BinarySerializer serializer)
        {
            base.SerializeVariables(serializer);
            if (serializer.Mode == SerializerMode.Write)
            {
                Variable.WriteVariable(serializer, Sampler);
                Variable.WriteVariable(serializer, Texture);
            }
            else
            {
                Sampler = Variable.ReadVariable(serializer);
                Texture = Variable.ReadVariable(serializer);
            }
        }
    }
}