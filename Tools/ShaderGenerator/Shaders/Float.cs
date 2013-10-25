using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public static class Float
    {
        public static Vector ShadowBias
        {
            get { return new Vector { Name = Param.Floats.ShadowBias, Type = Type.Float, ShaderReference = new ShaderReference(EngineReference.FloatSpriteSize) }; }
        }

        public static Vector SpriteSize
        {
            get { return new Vector { Name = Param.Floats.SpriteSize, Type = Type.Float, ShaderReference = new ShaderReference(EngineReference.FloatSpriteSize) }; }
        }
    }
}
