using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public class TexOffset : MethodBase
    {
        public TexOffset()
        {
            Name = Methods.TexOffset;
            U = new Vector { Name = "u", Type = Type.Float };
            V = new Vector { Name = "v", Type = Type.Float };

            // Shadows
            RegisterSignature(new MethodSignature(this, new TechniqueKey(vs: VertexShaderFlags.All, ps: PixelShaderFlags.All, sm: ShaderModel.Any),
                new[] { Type.Float.ToString(), Type.Float.ToString() },
                new[] { Floats.tU, Floats.tV }, Type.Float2));
        }

        public override string Body
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine("\treturn float2( u * 1.0f/1024, v * 1.0f/1024 );");
                sb.AppendLine("}");
                return sb.ToString();
            }
        }

        //public override string Signature
        //{
        //    get { return string.Format("{0} {1}({2} {3}, {4} {5})", Mapper.Map(ReturnType), Name, Mapper.Map(U.Type), U.Name, Mapper.Map(V.Type), V.Name); }
        //}

        [SupportedType(Type.Float)]
        public IVariable U { get; set; }

        [SupportedType(Type.Float)]
        public IVariable V { get; set; }
    }
}