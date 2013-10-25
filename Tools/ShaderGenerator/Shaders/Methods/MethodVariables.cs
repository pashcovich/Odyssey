using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public abstract partial class MethodBase
    {
        internal struct Floats
        {
            internal const string ShadowBias = "fBias";
            internal const string ShadowFactor = "fShadowFactor";
            internal const string SpriteSize = "fSpriteSize";
        }

        internal struct Samplers
        {
            internal const string sLinear = "sLinear";
            internal const string sShadowMap = "sShadowMap";
        }

        internal struct Structs
        {
            internal const string light = "light";
            internal const string material = "material";
        }

        internal struct Textures
        {
            internal const string tDiffuse = "tDiffuseMap";
            internal const string tShadowMap = "tShadowMap";
        }

        internal struct Vectors
        {
            internal const string vLightDirection = "vLightDirection";
            internal const string vNormal = "vNormal";
            internal const string vViewDirection = "vViewDirection";
            internal const string vDiffuseMapCoordinates = "vDiffuseMapCoordinates";
            internal const string vShadowProjection = "vShadowProjection";
            internal const string Position = "vPosition";
            internal const string PositionInstance = "pInstance";
            internal const string ScreenSize = "vScreenSize";
        }
    }
}
