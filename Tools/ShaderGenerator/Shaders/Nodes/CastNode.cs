using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    public enum Swizzle
    {
        X,
        Y,
        Z,
        W
    }

    [DataContract]
    public class CastNode : NodeBase
    {

        [DataMember]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Matrix)]
        public INode Input { get; set; }

        [DataMember]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float3x3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Matrix)]
        public override IVariable Output { get; set; }

        [DataMember]
        public float VectorWValue { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Input;
                foreach (var node in Input.DescendantNodes)
                    yield return node;
            }
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        public override string Access()
        {
            Type input = Input.Output.Type;
            Type output = Output.Type;

            switch (input)
            {
                case Type.Matrix:
                    switch (output)
                    {
                        case Type.Float3x3:
                            return string.Format("(float3x3){0}", Input.Reference);
                    }
                    break;


                case Type.Float3:
                    switch (output)
                    {
                        case Type.Float2:
                            return string.Format("{0}.{1}", Input.Reference, ((Vector)Output).PrintSwizzle());
                        case Type.Float4:
                            return string.Format("float4({0}.{1}, {2})", Input.Reference, ((Vector)Output).PrintSwizzle(), VectorWValue);
                    }
                    break;

                case Type.Float4:
                    switch (output)
                    {
                        case Type.Float2:
                        case Type.Float3:
                            return string.Format("{0}.{1}", Input.Reference, ((Vector)Output).PrintSwizzle());
                    }
                    break;

            }

            throw new InvalidOperationException("Unsupported cast operation.");
        }

        public static CastNode WorldInverseFloat3x3
        {
            get
            {

                return new CastNode
                {
                    Input = new ConstantNode { Value = Matrix.WorldInverse },
                    Output = new Matrix {  Name = "f3x3WorldInverse", Type = Type.Float3x3}
                };
            }
        }




    }
}
