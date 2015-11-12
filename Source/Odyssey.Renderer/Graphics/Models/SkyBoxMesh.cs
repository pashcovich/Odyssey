using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class SkyboxMesh : IPrimitiveGenerator<VertexPositionNormalTextureCube>
    {
        private readonly Vector3 tileFactor;
        protected Vector3 TileFactor { get {return tileFactor;} }

        protected SkyboxMesh(float tileX, float tileY, float tileZ)
        {
            tileFactor = new Vector3(tileX, tileY, tileZ);
        }

        public void GenerateMesh(out VertexPositionNormalTextureCube[] vertices, out int[] indices)
        {
            Vector3 vExtents = new Vector3(1, 1, 1);
            vertices = new VertexPositionNormalTextureCube[24];

            indices = new int[36];

            for (int x = 0; x < 6; x++)
            {
                indices[x * 6 + 0] = (ushort)(x * 4 + 0);
                indices[x * 6 + 1] = (ushort)(x * 4 + 2);
                indices[x * 6 + 2] = (ushort)(x * 4 + 1);

                indices[x * 6 + 3] = (ushort)(x * 4 + 0);
                indices[x * 6 + 4] = (ushort)(x * 4 + 3);
                indices[x * 6 + 5] = (ushort)(x * 4 + 2);
            }

            #region Back Face

            vertices[0] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, -vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(1.0f, -1.0f, -1.0f) * TileFactor
            };
            vertices[1] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(1.0f, 1.0f, -1.0f) * TileFactor
            };
            vertices[2] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(-1.0f, 1.0f, -1.0f) * TileFactor
            };
            vertices[3] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, -vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(-1.0f, -1.0f, -1.0f) * TileFactor
            };

            #endregion Back Face

            #region Front face

            vertices[4] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, -vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(-1.0f, -1.0f, 1.0f) * TileFactor
            };

            vertices[5] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(-1.0f, 1.0f, 1.0f) * TileFactor
            };
            vertices[6] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(1.0f, 1.0f, 1.0f) * TileFactor
            };
            vertices[7] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, -vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(1.0f, -1.0f, 1.0f) * TileFactor
            };

            #endregion Front face

            #region Bottom face

            vertices[8] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, -vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(-1.0f, -1.0f, -1.0f) * TileFactor
            };
            vertices[9] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, -vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(-1.0f, -1.0f, 1.0f) * TileFactor
            };
            vertices[10] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, -vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(1.0f, -1.0f, 1.0f) * TileFactor
            };
            vertices[11] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, -vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(1.0f, -1.0f, -1.0f) * TileFactor
            };

            #endregion Bottom face

            #region Top face

            vertices[12] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(1.0f, 1.0f, -1.0f) * TileFactor
            };
            vertices[13] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(1.0f, 1.0f, 1.0f) * TileFactor
            };

            vertices[14] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(-1.0f, 1.0f, 1.0f) * TileFactor
            };

            vertices[15] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(-1.0f, 1.0f, -1.0f) * TileFactor
            };

            #endregion Top face

            #region Left face

            vertices[16] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(-1.0f, 1.0f, -1.0f) * TileFactor
            };

            vertices[17] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(-1.0f, 1.0f, 1.0f) * TileFactor
            };

            vertices[18] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, -vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(-1.0f, -1.0f, 1.0f) * TileFactor
            };

            vertices[19] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(-vExtents.X, -vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(-1.0f, -1.0f, -1.0f) * TileFactor
            };

            #endregion Left face

            #region Right face

            vertices[20] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, -vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(1.0f, -1.0f, -1.0f) * TileFactor
            };

            vertices[21] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, -vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(1.0f, -1.0f, 1.0f) * TileFactor
            };

            vertices[22] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, vExtents.Y, vExtents.Z),
                TextureUVW = new Vector3(1.0f, 1.0f, 1.0f) * TileFactor
            };

            vertices[23] = new VertexPositionNormalTextureCube
            {
                Position = new Vector3(vExtents.X, vExtents.Y, -vExtents.Z),
                TextureUVW = new Vector3(1.0f, 1.0f, -1.0f) * TileFactor
            };

            #endregion Right face

        }

        public static Model New(DirectXDevice device, float tileX = 1.0f, float tileY = 1.0f, float tileZ = 1.0f, 
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList,
            ResourceUsage usage = ResourceUsage.Default,
            ModelOperation modelOperations = ModelOperation.None)
        {
            VertexPositionNormalTextureCube[] vertices;
            int[] indices;
            var skybox = new SkyboxMesh(tileX, tileY, tileZ);
            skybox.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive<VertexPositionNormalTextureCube>.New(device, "SkyBoxMesh",
                    vertices, indices, primitiveTopology, usage, modelOperations);
        }


    }
}
