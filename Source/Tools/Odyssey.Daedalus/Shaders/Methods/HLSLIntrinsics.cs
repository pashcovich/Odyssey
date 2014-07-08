using Odyssey.Graphics.Shaders;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public static class HlslIntrinsics
    {
        internal static string[] Intrinsics = { "normalize", "dot", "ddx", "ddy", "fwdith", "lerp", "max", "min", "step", "exp2" };

        static MethodSignature CreateSignature(IntrinsicFunction function, int numParameters, Type argumentType)
        {
            string[] argumentTypes = new string[numParameters];
            string[] arguments = new string[numParameters];
            for (int i = 0; i < argumentTypes.Length; i++)
            {
                argumentTypes[i] = argumentType.ToString();
                arguments[i] = "args" + ++i;
            }

            return new MethodSignature(function, new TechniqueKey(), argumentTypes, arguments, argumentType);
        }

        public static IMethod Normalize
        {
            get
            {
                var normalize = new IntrinsicFunction("normalize", 1);

                normalize.RegisterSignature(CreateSignature(normalize, 2, Type.Vector));
                
                return normalize;
            }
        }

        public static IMethod Dot
        {
            get
            {
                var dot = new IntrinsicFunction("dot", 2);
                dot.RegisterSignature(CreateSignature(dot, 2, Type.Vector));

                return dot;
            }
        }

        public static IMethod Ddx
        {
            get
            {
                var ddx = new IntrinsicFunction("ddx", 1);
                ddx.RegisterSignature(CreateSignature(ddx, 1, Type.Vector));

                return ddx;
            }
        }

        public static IMethod Ddy
        {
            get
            {
                var ddy = new IntrinsicFunction("ddy", 1);
                ddy.RegisterSignature(CreateSignature(ddy, 1, Type.Vector));

                return ddy;
            }
        }

        public static IMethod Lerp
        {
            get
            {
                var lerp = new IntrinsicFunction("lerp", 3);
                lerp.RegisterSignature(CreateSignature(lerp, 3, Type.Vector));
                return lerp;
            }
        }

        public static IMethod Max
        {
            get
            {
                var max = new IntrinsicFunction("max", 2);
                max.RegisterSignature(CreateSignature(max, 2, Type.Vector));
                
                return max;
            }
        }

        public static IMethod Min
        {
            get
            {
                var min = new IntrinsicFunction("min", 2); 
                min.RegisterSignature(CreateSignature(min, 2, Type.Vector));
                return min;
            }
        }

        public static IMethod Saturate
        {
            get
            {
                var step = new IntrinsicFunction("saturate", 1);
                step.RegisterSignature(CreateSignature(step, 1, Type.Vector));
                return step;
            }
        }

        public static IMethod Step
        {
            get
            {
                var step = new IntrinsicFunction("step", 2);
                step.RegisterSignature(CreateSignature(step, 2, Type.Vector));
                return step;
            }
        }

        public static IMethod Fwidth
        {
            get
            {
                var fwidth = new IntrinsicFunction("fwidth", 1);
                fwidth.RegisterSignature(CreateSignature(fwidth, 1, Type.Vector));
                return fwidth;
            }
        }

        public static IMethod Smoothstep
        {
            get
            {
                var smoothstep = new IntrinsicFunction("smoothstep", 3);
                smoothstep.RegisterSignature(CreateSignature(smoothstep, 3, Type.Vector));
                return smoothstep;
            }
        }

        public static IMethod Exp2
        {
            get
            {
                var smoothstep = new IntrinsicFunction("exp2", 1);
                smoothstep.RegisterSignature(CreateSignature(smoothstep, 1, Type.Vector));
                return smoothstep;
            }
        }
    }
}
