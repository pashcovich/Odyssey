using Odyssey.Serialization;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public class CastNode : NodeBase
    {
        private string[] mask;

        #region Static Methods

        public static CastNode WorldInverseTransposeFloat3x3
        {
            get
            {
                return CastWorldFloat3x3(Matrix.EntityWorldInverseTranspose, "mWorldInvT3x3");
            }
        }

        public static CastNode CastWorldFloat3x3(Matrix mWorld, string name)
        {
            return new CastNode
            {
                Input = new ReferenceNode { Value = mWorld },
                Output = new Matrix { Name = name, Type = Type.Float3x3 },
            };
        }

        public static CastNode WorldFloat3x3
        {
            get
            {
                return CastWorldFloat3x3(Matrix.EntityWorld, "mWorld3x3");
            }
        }

        public static CastNode PositionV3toV4(IVariable position)
        {
            return new CastNode
            {
                Input = new SwizzleNode { Input = new ReferenceNode { Value = position }, Swizzle = new[] { Swizzle.X, Swizzle.Y, Swizzle.Z, Swizzle.Null } },
                Output = new Vector { Type = Shaders.Type.Float4, Name = "vPosition" },
                Mask = new[] { "0", "0", "0", "1" },
                IsVerbose = true
            };
        }

        #endregion Static Methods

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

        [IgnoreValidation(true)]
        public string[] Mask
        {
            get { return mask; }
            set { mask = value; }
        }

        public override string Access()
        {
            Type input = Input.Output.Type;
            Type output = Output.Type;

            SwizzleNode nSwizzle = Input as SwizzleNode;

            switch (input)
            {
                case Type.Matrix:
                    switch (output)
                    {
                        case Type.Float3x3:
                            return $"(float3x3){Input.Reference}";
                    }
                    break;

                case Type.Float3:
                    switch (output)
                    {
                        case Type.Float2:
                            if (nSwizzle.Swizzle.Length != 2)
                                throw new InvalidOperationException("Input node is not a Swizzle node.");
                            return $"{Input.Reference}";

                        case Type.Float4:
                            return nSwizzle != null
                                ? $"float4({Input.Output.FullName}.{MaskVector(nSwizzle.Swizzle, Mask)})"
                                : $"float4({Input.Reference}, {Mask[0]})";
                    }
                    break;

                case Type.Float4:
                    switch (output)
                    {
                        case Type.Float2:
                        case Type.Float3:
                            if (nSwizzle.Swizzle.Length != 3)
                                throw new InvalidOperationException("Input node is not a Swizzle node.");
                            return $"{Input.Reference}";
                    }
                    break;
            }

            throw new InvalidOperationException("Unsupported cast operation.");
        }

        private static string MaskVector(Swizzle[] swizzle, string[] mask)
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
                    s += mask[i];
                }
            }

            return s;
        }

        protected override void RegisterNodes()
        {
            AddNode("Input", Input);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            serializer.Serialize(ref mask, serializer.Serialize);
        }
    }
}