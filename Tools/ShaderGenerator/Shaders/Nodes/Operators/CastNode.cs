using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators
{
    [YamlMapping(typeof(YamlCastNode))]
    [DataContract]
    public class CastNode : NodeBase
    {
        public static CastNode WorldInverseFloat3x3
        {
            get
            {
                return new CastNode
                {
                    Input = new ConstantNode { Value = Matrix.WorldInverse },
                    Output = new Matrix { Name = "f3x3WorldInverse", Type = Type.Float3x3 }
                };
            }
        }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Input;
                foreach (var node in Input.DescendantNodes)
                    yield return node;
            }
        }

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
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [IgnoreValidation(true)]
        public IVariable Mask { get; set; }

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
                            return string.Format("float4({0}.{1})", Input.Reference, MaskVector(((Vector)Output).Swizzle, (Vector)Mask));
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

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        static string MaskVector(Swizzle[] swizzle, Vector mask)
        {
            Contract.Requires<ArgumentNullException>(swizzle != null);
            Contract.Requires<ArgumentException>(swizzle.Length > 0);
            Contract.Requires<ArgumentNullException>(mask != null);
            string s = string.Empty;
            for (int i = 0; i < swizzle.Length; i++)
            {
                Swizzle component = swizzle[i];
                if (component != Swizzle.Null)
                {
                    s += component.ToString().ToLowerInvariant();
                    if (i + 1 < swizzle.Length - 2 && swizzle[i + 1] == Swizzle.Null)
                        s += ", ";
                }
                else
                {
                    if (i > 0)
                        s += ", ";
                    s += mask.Value[i];
                }
            }

            return s;
        }
    }
}