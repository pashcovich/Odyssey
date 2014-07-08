using Odyssey.Engine;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using SharpDX.Serialization;
using System.Collections.Generic;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    public class ClipSpaceTransformNode : NodeBase
    {
        private IMethod clipSpaceMethod;
        private IVariable output;

        [SupportedType(Type.Float3)]
        public IVariable InstancePosition { get; set; }

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
            set
            {
                output = value;
            }
        }

        [SupportedType(Type.Float3)]
        public INode Position { get; set; }

        public override IEnumerable<IMethod> RequiredMethods { get { yield return clipSpaceMethod; } }

        [SupportedType(Type.Float2)]
        public IVariable ScreenSize { get; set; }

        [SupportedType(Type.Float2)]
        public IVariable Size { get; set; }

        public override string Access()
        {
            return clipSpaceMethod.Call((Vector)Position.Output, (Vector)InstancePosition, (Vector)Size, (Vector)ScreenSize);
        }

        public override void Validate(TechniqueKey key)
        {
            base.Validate(key);
            if (clipSpaceMethod == null)
                clipSpaceMethod = new ClipSpaceTransform();

            clipSpaceMethod.ActivateSignature(key);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);

            if (serializer.Mode == SerializerMode.Write)
            {
                Variable.WriteVariable(serializer, InstancePosition);
                // TODO Serialize MethodBase
            }
            else
                InstancePosition = Variable.ReadVariable(serializer);
        }

        protected override void RegisterNodes()
        {
            Nodes.Add("Position", Position);
        }

        public static DeclarationNode FullScreenNode(IStruct inputStruct, IVariable viewportSize)
        {
            return new DeclarationNode
            {
                Inputs = new List<INode>
                {
                new SubtractionNode
                {
                    Input1 = new DivisionNode{
                        Input1 = new MultiplyNode
                        {
                            Input1 = new ScalarNode {Value = 2},
                            Input2 = new MultiplyNode
                            {
                                Input1 = new SwizzleNode
                                {
                                    Input = new ReferenceNode {Value = inputStruct[Param.SemanticVariables.ObjectPosition]},
                                    Swizzle = new [] { Swizzle.X}
                                },
                                Input2 = new SwizzleNode
                                {
                                    Input = new ReferenceNode {Value = viewportSize},
                                    Swizzle = new [] { Swizzle.X}
                                },
                                Parenthesize = true
                            },
                        },
                        Input2 = new SwizzleNode
                        {
                            Input = new ReferenceNode { Value = viewportSize },
                            Swizzle = new[] { Swizzle.X}
                        },
                    },
                    Input2 = new ScalarNode {  Value = 1},
                },
                new SubtractionNode
                {
                    Input1 = new ScalarNode {Value = 1},
                    Input2 = new MultiplyNode
                    {
                        Input1 = new ScalarNode {Value = 2},
                        Input2 = new DivisionNode
                        {
                            Input1 = new MultiplyNode
                            {
                                Input1 = new SwizzleNode
                                    {
                                        Input = new ReferenceNode {Value = inputStruct[Param.SemanticVariables.ObjectPosition]},
                                        Swizzle = new [] { Swizzle.Y}
                                    },
                                Input2 = new SwizzleNode
                                    {
                                        Input = new ReferenceNode {Value = viewportSize},
                                        Swizzle = new [] { Swizzle.Y}
                                    },
                                Parenthesize = true,
                            },
                            Input2 = new SwizzleNode
                            {
                                Input = new ReferenceNode { Value = viewportSize },
                                Swizzle = new[] { Swizzle.Y}
                            },
                        }
                    }
                },
                new ScalarNode {Value =  0},
                new ScalarNode { Value = 1}
                },
                Output = new Vector { Type = Shaders.Type.Float4, Name = "pClip" }
            };
        }
    }
}