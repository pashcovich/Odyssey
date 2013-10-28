using Odyssey.Content.Shaders;
using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public partial class Variable 
    {
        public static Variable Local
        {
            get { return new Variable { Type = Type.None, Name = "Local", Semantic = Semantic.Null, Index = -1 }; }
        }



        public static Variable Color
        {
            get { return new Variable() { Type = Type.Float4, Name = Param.SemanticVariables.Color, Semantic = Semantic.Color, Index = -1 }; }
        }

        public static Variable ColorPSOut
        {
            get { return new Variable() { Type = Type.Float4, Name = Param.SemanticVariables.Color, Semantic = Semantic.SV_Target, Index = -1 }; }
        }

        


    }
}
