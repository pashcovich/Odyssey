using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math
{
    [YamlMapping(typeof(YamlMatrixMultiplyNode))]
    public class MatrixMultiplyNode : NodeBase
    {
        IVariable output;

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Float3x3)]
        [SupportedType(Type.Matrix)]
        public INode Input1 { get; set; }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Float3x3)]
        [SupportedType(Type.Matrix)]
        public INode Input2 { get; set; }

        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Float3x3)]
        [SupportedType(Type.Matrix)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    string id = string.Empty;
                    Type type = MultiplyResultType(Input1.Output.Type, Input2.Output.Type);

                    string name1 = Char.IsLower(Input1.Output.Name[0]) ? Input1.Output.Name.Substring(1, Input1.Output.Name.Length-1) : Input1.Output.Name;
                    string name2 = Char.IsLower(Input2.Output.Name[0]) ? Input2.Output.Name.Substring(1, Input2.Output.Name.Length-1) : Input2.Output.Name;

                    Output = Variable.InitVariable(string.Format("{0}{1}{2}", Variable.GetPrefix(type), name1, name2), type);
                }
                return output;
            }
            set { output = value; }
        }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return Input1;
                foreach (var node in Input1.DescendantNodes)
                    yield return node;

                yield return Input2;
                foreach (var node in Input2.DescendantNodes)
                    yield return node;
            }
        }

        public override void Validate(TechniqueKey key)
        {
            if (Output.Type != MultiplyResultType(Input1.Output.Type, Input2.Output.Type))
                throw new InvalidOperationException("Type mismatch between multiplication factors and Output.");
            base.Validate(key);

        }

        internal static Type MultiplyResultType(Type x, Type y)
        {
            switch (x)
            {
                case Type.Float:
                    switch (y)
                    {
                        case Type.Float:
                            return Type.Float;

                        case Type.Float2:
                        case Type.Float3:
                        case Type.Float4:
                            return y;

                        case Type.Matrix:
                            return Type.Matrix;
                    }
                    break;


                case Type.Float3:
                    switch (y)
                    {
                        case Type.Float:
                            return y;

                        case Type.Float2:
                        case Type.Float3:
                        case Type.Float4:
                            return Type.Float;

                        case Type.Matrix:
                            return Type.Float4;

                        case Type.Float3x3:
                            return Type.Float3;
                    }
                    break;

                case Type.Float2:
                case Type.Float4:
                    switch (y)
                    {
                        case Type.Float:
                            return y;

                        case Type.Float2:
                        case Type.Float3:
                        case Type.Float4:
                            return Type.Float;

                        case Type.Matrix:
                            return Type.Float4;
                    }
                    break;

                case Type.Matrix:
                    switch (y)
                    {
                        case Type.Float:
                        case Type.Matrix:
                            return Type.Matrix;

                        case Type.Float2:
                        case Type.Float3:
                        case Type.Float4:
                            return y;
                    }
                    break;

            }

            return Type.None;
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        public override string Access()
        {
            return string.Format("mul({0}, {1})", Input1.Reference, Input2.Reference);
        }

        public static MatrixMultiplyNode WorldViewProjection
        {
            get
            {
                MatrixMultiplyNode mulWV = new MatrixMultiplyNode
                {
                    Input1 = new ConstantNode { Value = Matrix.World },
                    Input2 = new ConstantNode { Value = Matrix.View},
                };

                MatrixMultiplyNode mulWVP = new MatrixMultiplyNode
                {
                    Input1 = mulWV,
                    Input2 = new ConstantNode { Value = Matrix.Projection},
                    IsVerbose = true
                };

                return mulWVP;
            }
        }

        public static MatrixMultiplyNode LightWorldViewProjection
        {
            get
            {
                MatrixMultiplyNode mulVP = new MatrixMultiplyNode
                {
                    Input1 = new ConstantNode { Value = Matrix.LightView },
                    Input2 = new ConstantNode { Value = Matrix.LightProjection },
                };

                MatrixMultiplyNode mulLightWVP = new MatrixMultiplyNode
                {
                    Input1 = new ConstantNode { Value = Matrix.World },
                    Input2 = mulVP,
                    IsVerbose = true
                };

                return mulLightWVP;
            }
        }


    }
}
