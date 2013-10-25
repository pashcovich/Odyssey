using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Materials
{
    public enum ReferenceType
    {
        None,
        Engine,
        Texture
    }

    public enum EngineReference
    {
        None,
        FloatSpriteSize,
        LightPoint0VS,
        LightPoint0PS,
        Material,
        MaterialDiffuse,
        MatrixLightView,
        MatrixLightProjection,
        MatrixProjection,
        MatrixView,
        MatrixWorld,
        MatrixWorldInverse,
        MatrixWorldInverseTranspose,
        VectorCameraPosition,
        VectorScreenSize,
        VectorSpritePosition,
        
    }

    public enum TextureReference
    {
        Diffuse,
        Normal,
        ShadowMap,
        Procedural,
    }
}
