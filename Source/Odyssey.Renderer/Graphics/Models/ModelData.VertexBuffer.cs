using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Serialization;

namespace Odyssey.Graphics.Models
{
    public sealed partial class ModelData
    {
        public sealed class VertexBuffer : IDataSerializable
        {
            /// <summary>
            /// The number of vertices stored in this vertex buffer.
            /// </summary>
            public int Count;

            /// <summary>
            /// The layout of the vertex buffer.
            /// </summary>
            public List<VertexElement> Layout;

            /// <summary>
            /// Gets the vertex buffer for this mesh part.
            /// </summary>
            public byte[] Buffer;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Count);
                serializer.Serialize(ref Layout);
                serializer.Serialize(ref Buffer);
            }
        }
    }
}
