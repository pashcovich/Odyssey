using System.Collections.Generic;
using Odyssey.Content;
using SharpDX.Mathematics;
using Odyssey.Serialization;

namespace Odyssey.Graphics.Models
{
    public sealed partial class ModelData
    {
        /// <summary>
        /// Class Mesh
        /// </summary>
        public sealed class Mesh : IDataSerializable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mesh"/> class.
            /// </summary>
            public Mesh()
            {
                VertexBuffers = new List<VertexBuffer>();
                IndexBuffers = new List<IndexBuffer>();
                MeshParts = new List<MeshPart>();
                Properties = new PropertyCollection();
            }

            /// <summary>
            /// Gets the name of this mesh.
            /// </summary>
            public string Name;

            /// <summary>
            /// The bounding sphere for this mesh (in local object space).
            /// </summary>
            public BoundingSphere BoundingSphere;

            /// <summary>
            /// Gets the shared vertex buffers
            /// </summary>
            public List<VertexBuffer> VertexBuffers;

            /// <summary>
            /// Gets the shared index buffers
            /// </summary>
            public List<IndexBuffer> IndexBuffers;

            /// <summary>
            /// Gets the <see cref="MeshPart"/> instances that make up this mesh. Each part of a mesh is composed of a set of primitives that share the same material. 
            /// </summary>
            public List<MeshPart> MeshParts;

            /// <summary>
            /// Gets attributes attached to this mesh.
            /// </summary>
            public PropertyCollection Properties;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name, false, SerializeFlags.Nullable);
                serializer.Serialize(ref BoundingSphere);
                serializer.Serialize(ref VertexBuffers);
                serializer.Serialize(ref IndexBuffers);
                serializer.Serialize(ref MeshParts);
                serializer.Serialize(ref Properties);
            }
        }
    }
}
