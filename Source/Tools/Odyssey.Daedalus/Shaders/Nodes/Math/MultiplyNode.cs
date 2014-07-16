using Odyssey.Graphics.Shaders;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Odyssey.Daedalus.Shaders.Nodes.Math
{
    public class MultiplyNode : MathNodeBase
    {
        public override void Validate(TechniqueKey key)
        {
            Contract.Requires<InvalidOperationException>(Output.Type == MultiplyResultType(Input1.Output.Type, Input2.Output.Type), "Type mismatch between factors.");
            base.Validate(key);
        }

        protected override Type GetResultType(Type factor1, Type factor2)
        {
            return MultiplyResultType(factor1, factor2);
        }

        protected override char GetOperator()
        {
            const char mul = '*';
            return mul;
        }

        public static Type MultiplyResultType(Type x, Type y)
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
                            return Type.Float3;

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
                    switch (y)
                    {
                        case Type.Float:
                        case Type.Float2:
                            return Type.Float2;

                        case Type.Float3:
                        case Type.Float4:
                            return Type.Float;
                    }
                    break;

                case Type.Float4:
                    switch (y)
                    {
                        case Type.Float:
                            return Type.Float4;

                        case Type.Float2:
                        case Type.Float3:
                            return Type.Float;

                        case Type.Float4:
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
    }
}