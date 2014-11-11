using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Content;
using SharpDX.Mathematics;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using Odyssey.Serialization;

namespace Odyssey.Graphics.Models
{
    public sealed partial class ModelData
    {
        public sealed class MeshPart : IDataSerializable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MeshPart"/> class.
            /// </summary>
            public MeshPart()
            {
                Properties = new PropertyCollection();
            }

            /// <summary>
            /// The index buffer range. The slot in the buffer range is the position of the index buffer in <see cref="ModelData.IndexBuffers"/>.
            /// </summary>
            public BufferRange IndexBufferRange;

            /// <summary>
            /// The vertex buffer range. The slot in the buffer range is the position of the vertex buffer in <see cref="ModelData.VertexBuffers"/>.
            /// </summary>
            public BufferRange VertexBufferRange;

            /// <summary>
            /// The attributes attached to this mesh part.
            /// </summary>
            public PropertyCollection Properties;

            public PrimitiveTopology PrimitiveTopology;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref IndexBufferRange);
                serializer.Serialize(ref VertexBufferRange);
                serializer.SerializeEnum(ref PrimitiveTopology);
                serializer.Serialize(ref Properties);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BufferRange : IDataSerializable
        {
            public int Slot;

            public int Start;

            public int Count;

            public void Serialize(BinarySerializer serializer)
            {
                if (serializer.Mode == SerializerMode.Read)
                {
                    var reader = serializer.Reader;
                    Slot = reader.ReadInt32();
                    Start = reader.ReadInt32();
                    Count = reader.ReadInt32();
                }
                else
                {
                    var writer = serializer.Writer;
                    writer.Write(Slot);
                    writer.Write(Start);
                    writer.Write(Count);
                }
            }
        }
    }
}
