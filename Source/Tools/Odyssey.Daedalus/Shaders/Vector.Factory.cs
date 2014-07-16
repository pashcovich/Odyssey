using Odyssey.Daedalus.Data;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.Shaders
{
    public partial class Vector
    {
        public static Vector CameraPosition
        {
            get
            {
                var v = new Vector
                {
                    Type = Type.Float3,
                    Name = Param.Vectors.CameraPosition,
                    EngineReference = ReferenceFactory.Camera.VectorPosition
                    
                };
                v.SetMarkup(Param.Properties.CameraId, 0);
                return v;
            }
        }

        public static Vector ViewportSize
        {
            get { return new Vector { Type = Type.Float2, Name = Param.Vectors.ViewportSize, EngineReference = ReferenceFactory.Application.ViewportSize }; }
        }

        public static Vector SpritePosition
        {
            get
            {
                return new Vector { Type = Type.Float3, Name = Param.Vectors.SpritePosition, EngineReference = ReferenceFactory.Effect.SpritePosition }; 
            }
        }

        public static Vector LightDirection
        {
            get
            {
                var v = new Vector { Type = Type.Float3, Name = Param.Vectors.LightDirection, EngineReference = ReferenceFactory.Light.VectorDirection };
                v.SetMarkup(Param.Properties.LightId, 0);
                return v;
            }
        }

        /// <summary>
        /// Returns a new <code>float4 Position: SV_POSITION</code> semantic variable to be uses within an output struct.
        /// It represents the position of each vertex in clip space.
        /// </summary>
        public static Vector ClipPosition
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.Position, Semantic = Semantics.SV_Position}; }
        }

        /// <summary>
        /// Returns a new <code>float3 ObjectPosition: POSITION</code> semantic variable to be used within an input struct. 
        /// It represents the position of each vertex in object space.
        /// </summary>
        public static Vector ObjectPosition
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.ObjectPosition, Semantic = Semantics.Position}; }
        }

        /// <summary>
        /// Returns a new <code>float 3 WorldPosition: POSITION</code> semantic variable to be used within an output struct.
        /// It represents the position of each vertex in world space.
        /// </summary>
        public static Vector WorldPosition
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.WorldPosition, Semantic = Semantics.Position }; }
        }
        /// <summary>
        /// Returns a new <code>float 4 WorldPosition: POSITION</code> semantic variable to be used within an output struct.
        /// It represents the position of each vertex in world space.
        /// </summary>
        public static Vector WorldPosition4
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.WorldPosition, Semantic = Semantics.Position}; }
        }

        public static Vector Normal
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.Normal, Semantic = Semantics.Normal}; }
        }

        public static Vector TextureUV
        {
            get { return new Vector { Type = Type.Float2, Name = Param.SemanticVariables.Texture, Semantic = Semantics.Texcoord}; }
        }

        public static Vector Barycentric
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.Barycentric, Semantic = Semantics.Barycentric }; }
        }

        public static Vector Intensity
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.Intensity, Semantic = Semantics.Intensity}; }
        }

        public static Vector Tangent
        {
            get { return new Vector {Type = Type.Float4, Name= Param.SemanticVariables.Tangent, Semantic = Semantics.Tangent}; }
        }

        public static Vector TextureUVW
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.Texture, Semantic = Semantics.Texcoord }; }
        }

        public static Vector ViewDirection
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.ViewDirection, Semantic = Semantics.ViewDirection}; }
        }

        public static Vector Color
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.Color, Semantic = Semantics.Color}; }
        }

        public static Vector ColorPSOut
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.Color, Semantic = Semantics.SV_Target }; }
        }

        public static Vector ShadowProjection
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.ShadowProjection, Semantic = Semantics.Texcoord }; }
        }

        public static Vector SpriteSize
        {
            get { return new Vector { Name = Param.Vectors.SpriteSize, Type = Type.Float2, EngineReference = ReferenceFactory.Effect.SpriteSize }; }
        }
    }
}
