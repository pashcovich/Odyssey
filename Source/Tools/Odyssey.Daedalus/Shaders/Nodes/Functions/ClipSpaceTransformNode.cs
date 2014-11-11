using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Daedalus.Shaders.Nodes.Math;
using Odyssey.Daedalus.Shaders.Nodes.Operators;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Shaders;
using Odyssey.Serialization;
using System.Collections.Generic;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
{
    public class ClipSpaceTransformNode : NodeBase
    {
        private IMethod clipSpaceMethod;
        private IVariable output;

        [SupportedType(Type.Float3)]
        public IVariable InstancePosition { get; set; }

        [SupportedType(Type.Vector)]
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

        protected override void SerializeMethods(BinarySerializer serializer)
        {
            base.SerializeMethods(serializer);
            if (serializer.Mode == SerializerMode.Read)
                MethodBase.WriteMethod(serializer, clipSpaceMethod);
            else clipSpaceMethod = MethodBase.ReadMethod(serializer);
        }

        protected override void SerializeVariables(BinarySerializer serializer)
        {
            base.SerializeVariables(serializer);
            if (serializer.Mode == SerializerMode.Write)
                Variable.WriteVariable(serializer, InstancePosition);
            else
                InstancePosition = Variable.ReadVariable(serializer);
        }

        protected override void RegisterNodes()
        {
            AddNode("Position", Position);
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