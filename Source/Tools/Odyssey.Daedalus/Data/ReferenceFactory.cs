using Odyssey.Engine;

namespace Odyssey.Daedalus.Data
{
    public static class ReferenceFactory
    {
        public struct Application
        {
            private const string ApplicationKey = "Application";
            public static readonly EngineReference ViewportSize = new EngineReference(ApplicationKey, Reference.Application.ViewportSize);
        }

        public struct Camera
        {
            private const string CameraKey = "Camera";
            public static readonly EngineReference MatrixView = new EngineReference(CameraKey, Reference.Matrix.View);
            public static readonly EngineReference MatrixProjection = new EngineReference(CameraKey, Reference.Matrix.Projection);
            public static readonly EngineReference VectorPosition = new EngineReference(CameraKey, Reference.Vector.Position);
        }


        public struct Light
        {
            private const string LightKey = "Light";
            public static readonly EngineReference VectorDirection = new EngineReference(LightKey, Reference.Vector.Direction);
            public static readonly EngineReference StructPointVS = new EngineReference(LightKey, Reference.Struct.PointLightVS);
            public static readonly EngineReference StructPointPS = new EngineReference(LightKey, Reference.Struct.PointLightPS);
            public static readonly EngineReference MatrixView = new EngineReference(LightKey, Reference.Matrix.View);
            public static readonly EngineReference MatrixProjection = new EngineReference(LightKey, Reference.Matrix.Projection);
        }

        public struct Material
        {
            private const string MaterialKey = "Material";
            public static readonly EngineReference StructMaterialPS = new EngineReference(MaterialKey, Reference.Struct.MaterialPS);
            public static readonly EngineReference ColorDiffuse = new EngineReference(MaterialKey, Reference.Color.Diffuse);
        }

        public struct Entity
        {
            private const string EntityKey = "Entity";
            public static readonly EngineReference MatrixWorld = new EngineReference(EntityKey, Reference.Matrix.World);
            public static readonly EngineReference MatrixWorldInverse = new EngineReference(EntityKey, Reference.Matrix.WorldInverse);
            public static readonly EngineReference MatrixWorldInverseTranspose = new EngineReference(EntityKey, Reference.Matrix.WorldInverseTranspose);
        }

        public struct Effect
        {
            private const string EffectKey = "Effect";
            public static readonly EngineReference BlurOffsetsWeights = new EngineReference(EffectKey, Reference.Effect.BlurOffsetWeights);
            public static readonly EngineReference BloomParameters = new EngineReference(EffectKey, Reference.Effect.BloomParameters);
            public static readonly EngineReference BloomThreshold = new EngineReference(EffectKey, Reference.Effect.BloomThreshold);
            public static readonly EngineReference GlowStrength = new EngineReference(EffectKey, Reference.Effect.GlowStrength);
            public static readonly EngineReference SpriteSize = new EngineReference(EffectKey, Reference.Effect.SpriteSize);
            public static readonly EngineReference SpritePosition = new EngineReference(EffectKey, Reference.Effect.SpritePosition);
        }

        public struct Texture
        {
            private const string TextureKey = "Texture";
            public static readonly EngineReference Diffuse = new EngineReference(TextureKey, Reference.Texture.Diffuse);
            public static readonly EngineReference Normal = new EngineReference(TextureKey,  Reference.Texture.Normal);
            public static readonly EngineReference Shadow = new EngineReference(TextureKey,  Reference.Texture.Shadow);
        }

    }
}
