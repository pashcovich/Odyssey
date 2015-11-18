using Odyssey.Engine;

namespace Odyssey.Daedalus.Data
{
    public static class ReferenceFactory
    {
        public struct Application
        {
            private const string ApplicationKey = "Application";
            public static EngineReference ViewportSize => new EngineReference(ApplicationKey, Reference.Application.ViewportSize);
        }

        public struct Camera
        {
            private const string CameraKey = "Camera";
            public static EngineReference MatrixView => new EngineReference(CameraKey, Reference.Matrix.View);
            public static EngineReference MatrixProjection => new EngineReference(CameraKey, Reference.Matrix.Projection);
            public static EngineReference VectorPosition => new EngineReference(CameraKey, Reference.Vector.Position);
        }


        public struct Light
        {
            private const string LightKey = "Light";
            public static EngineReference VectorDirection => new EngineReference(LightKey, Reference.Vector.Direction);
            public static EngineReference StructPointVS => new EngineReference(LightKey, Reference.Struct.PointLightVS);
            public static EngineReference StructPointPS => new EngineReference(LightKey, Reference.Struct.PointLightPS);
            public static EngineReference MatrixView => new EngineReference(LightKey, Reference.Matrix.View);
            public static EngineReference MatrixProjection => new EngineReference(LightKey, Reference.Matrix.Projection);
        }

        public struct Material
        {
            private const string MaterialKey = "Material";
            public static EngineReference StructMaterialPS => new EngineReference(MaterialKey, Reference.Struct.MaterialPS);
            public static EngineReference ColorDiffuse => new EngineReference(MaterialKey, Reference.Color.Diffuse);
        }

        public struct Entity
        {
            private const string EntityKey = "Entity";
            public static EngineReference MatrixWorld => new EngineReference(EntityKey, Reference.Matrix.World);
            public static EngineReference MatrixWorldInverse => new EngineReference(EntityKey, Reference.Matrix.WorldInverse);
            public static EngineReference MatrixWorldInverseTranspose => new EngineReference(EntityKey, Reference.Matrix.WorldInverseTranspose);
        }

        public struct Effect
        {
            private const string EffectKey = "Effect";
            public static EngineReference BlurOffsetsWeights => new EngineReference(EffectKey, Reference.Effect.BlurOffsetWeights);
            public static EngineReference BloomParameters => new EngineReference(EffectKey, Reference.Effect.BloomParameters);
            public static EngineReference BloomThreshold => new EngineReference(EffectKey, Reference.Effect.BloomThreshold);
            public static EngineReference GlowStrength => new EngineReference(EffectKey, Reference.Effect.GlowStrength);
            public static EngineReference SpriteSize => new EngineReference(EffectKey, Reference.Effect.SpriteSize);
            public static EngineReference SpritePosition => new EngineReference(EffectKey, Reference.Effect.SpritePosition);
        }

        public struct Texture
        {
            private const string TextureKey = "Texture";
            public static EngineReference Diffuse => new EngineReference(TextureKey, Reference.Texture.Diffuse);
            public static EngineReference Normal => new EngineReference(TextureKey,  Reference.Texture.Normal);
            public static EngineReference Shadow => new EngineReference(TextureKey,  Reference.Texture.Shadow);
        }
    }
}
