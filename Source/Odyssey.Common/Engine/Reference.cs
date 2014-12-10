namespace Odyssey.Engine
{
    internal static class Reference
    {
        internal struct Application
        {
            internal const string ViewportSize = "ViewportSize";
        }

        internal struct Color
        {
            internal const string Diffuse = "Diffuse";
        }

        internal struct Effect
        {
            internal const string BlurOffsetWeights = "BlurOffsetsWeights";
            internal const string BloomParameters = "BloomParameters";
            internal const string BloomThreshold = "BloomThreshold";
            internal const string GlowStrength = "GlowStrength";
            internal const string SpriteSize = "SpriteSize";
            internal const string SpritePosition = "SpritePosition";
        }

        internal struct Group
        {
            internal const string Application = "Application";
            internal const string Camera = "Camera";
            internal const string Material = "Material";
            internal const string Light = "Light";
            internal const string Entity = "Entity";
            internal const string Effect = "Effect";
        }

        internal struct Matrix
        {
            internal const string View = "MatrixView";
            internal const string World = "MatrixWorld";
            internal const string WorldInverse = "MatrixWorldInverse";
            internal const string WorldInverseTranspose = "MatrixWorldInverseTranspose";
            internal const string Projection = "MatrixProjection";
        }

        internal struct Struct
        {
            internal const string PointLightPS = "PointPS";
            internal const string PointLightVS = "PointVS";
            internal const string MaterialPS = "Material";
        }

        internal struct Texture
        {
            internal const string Diffuse = "Diffuse";
            internal const string Normal = "Normal";
            internal const string Shadow = "Shadow";
        }

        internal struct Vector
        {
            internal const string Position = "VectorPosition";
            internal const string Direction = "VectorDirection";
        }
    }
}