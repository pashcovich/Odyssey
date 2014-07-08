using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators
{
    public class CastNode : NodeBase
    {
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
        public string[] Mask { get; set; }

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
                            return string.Format("(float3x3){0}", Input.Reference);
                    }
                    break;

                case Type.Float3:
                    switch (output)
                    {
                        case Type.Float2:
                            if (nSwizzle.Swizzle.Length != 2)
                                throw new InvalidOperationException("Input node is not a Swizzle node.");
                            return string.Format("{0}", Input.Reference);

                        case Type.Float4:
                            return nSwizzle != null
                                ? string.Format("float4({0}.{1})", Input.Output.FullName, MaskVector(nSwizzle.Swizzle, Mask))
                                : string.Format("float4({0}, {1})", Input.Reference, Mask[0]);
                    }
                    break;

                case Type.Float4:
                    switch (output)
                    {
                        case Type.Float2:
                        case Type.Float3:
                            if (nSwizzle.Swizzle.Length != 3)
                                throw new InvalidOperationException("Input node is not a Swizzle node.");
                            return string.Format("{0}", Input.Reference);
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
            Nodes.Add("Input", Input);
        }
    }
}