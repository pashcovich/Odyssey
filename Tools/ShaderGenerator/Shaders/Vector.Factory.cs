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
    public partial class Vector
    {
        public static Vector CameraPosition
        {
            get { return new Vector { Type = Type.Float3, Name = Param.Vectors.CameraPosition, ShaderReference = new ShaderReference(EngineReference.VectorCameraPosition) }; }
        }

        public static Vector ScreenSize
        {
            get { return new Vector { Type = Type.Float2, Name = Param.Vectors.ScreenSize, ShaderReference = new ShaderReference(EngineReference.VectorScreenSize) }; }
        }

        public static Vector SpritePosition
        {
            get { return new Vector { Type = Type.Float3, Name = Param.Vectors.SpritePosition, ShaderReference = new ShaderReference(EngineReference.VectorSpritePosition) }; }
        }

        /// <summary>
        /// Returns a new <code>float4 Position: SV_POSITION</code> semantic variable to be uses within an output struct.
        /// It represents the position of each vertex in clip space.
        /// </summary>
        public static Vector ClipPosition
        {
            get { return new Vector() { Type = Type.Float4, Name = Param.SemanticVariables.Position, Semantic = Semantic.SV_Position}; }
        }

        /// <summary>
        /// Returns a new <code>float3 ObjectPosition: POSITION</code> semantic variable to be used within an input struct. 
        /// It represents the position of each vertex in object space.
        /// </summary>
        public static Vector ObjectPosition
        {
            get { return new Vector() { Type = Type.Float3, Name = Param.SemanticVariables.ObjectPosition, Semantic = Semantic.Position}; }
        }

        /// <summary>
        /// Returns a new <code>float 3 WorldPosition: POSITION</code> semantic variable to be used within an output struct.
        /// It represents the position of each vertex in world space.
        /// </summary>
        public static Vector WorldPosition
        {
            get { return new Vector() { Type = Type.Float3, Name = Param.SemanticVariables.WorldPosition, Semantic = Semantic.Position }; }
        }
        /// <summary>
        /// Returns a new <code>float 4 WorldPosition: POSITION</code> semantic variable to be used within an output struct.
        /// It represents the position of each vertex in world space.
        /// </summary>
        public static Vector WorldPosition4
        {
            get { return new Vector() { Type = Type.Float4, Name = Param.SemanticVariables.WorldPosition, Semantic = Semantic.Position}; }
        }

        public static Vector Normal
        {
            get { return new Vector { Type = Type.Float3, Name = Param.SemanticVariables.Normal, Semantic = Semantic.Normal}; }
        }

        public static Vector TextureUV
        {
            get { return new Vector() { Type = Type.Float2, Name = Param.SemanticVariables.Texture, Semantic = Semantic.Texcoord}; }
        }

        public static Vector TextureUVW
        {
            get { return new Vector() { Type = Type.Float3, Name = Param.SemanticVariables.Texture, Semantic = Semantic.Texcoord }; }
        }

        public static Vector ViewDirection
        {
            get { return new Vector() { Type = Type.Float4, Name = Param.SemanticVariables.ViewDirection, Semantic = Semantic.ViewDirection}; }
        }

        public static Vector Color
        {
            get { return new Vector() { Type = Type.Float4, Name = Param.SemanticVariables.Color, Semantic = Semantic.Color}; }
        }

        public static Vector ColorPSOut
        {
            get { return new Vector() { Type = Type.Float4, Name = Param.SemanticVariables.Color, Semantic = Semantic.SV_Target }; }
        }

        public static Vector ShadowProjection
        {
            get { return new Vector { Type = Type.Float4, Name = Param.SemanticVariables.ShadowProjection, Semantic = Semantic.Texcoord }; }
        }

    
    }
}
