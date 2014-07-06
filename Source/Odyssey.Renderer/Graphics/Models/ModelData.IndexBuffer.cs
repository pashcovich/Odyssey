using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Serialization;

namespace Odyssey.Graphics.Models
{
    public sealed partial class ModelData
    {
        public sealed class IndexBuffer : IDataSerializable
        {
            /// <summary>
            /// The number of indices stored in this index buffer.
            /// </summary>
            public int Count;

            /// <summary>
            /// Gets the index buffer for this mesh part.
            /// </summary>
            public byte[] Buffer;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Count);
                serializer.Serialize(ref Buffer);
            }
        }
    }
}
