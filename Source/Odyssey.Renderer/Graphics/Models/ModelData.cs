using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Content;
using Odyssey.Utilities.Text;
using SharpDX.IO;
using SharpDX.Serialization;

namespace Odyssey.Graphics.Models
{
    public sealed partial class ModelData : IDataSerializable
    {
        public const string OdysseyIdentifier = "OMDL";
        public const int Version = 0x100;
        private List<Mesh> meshData;

        public ModelData()
        {
            meshData = new List<Mesh>();
        }

        /// <summary>
        /// Gets the maximum buffer size in bytes that will be needed when loading this model.
        /// </summary>
        public int MaximumBufferSizeInBytes;

        /// <summary>
        /// Gets the mesh of this model.
        /// </summary>
        public List<Mesh> Meshes { get { return meshData; } }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            serializer.BeginChunk(OdysseyIdentifier);

            // Writes the version
            if (serializer.Mode == SerializerMode.Read)
            {
                int version = serializer.Reader.ReadInt32();
                if (version != Version)
                {
                    throw new NotSupportedException(string.Format("ModelData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, Version));
                }
            }
            else
            {
                serializer.Writer.Write(Version);
            }

            // Serialize the maximum buffer size used when loading this model.
            serializer.Serialize(ref MaximumBufferSizeInBytes);

            // Mesh section
            serializer.BeginChunk("MESH");
            serializer.Serialize(ref meshData);
            serializer.EndChunk();
            
            serializer.EndChunk();
        }

        /// <summary>
        /// Loads a <see cref="ModelData"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A <see cref="ModelData"/>. Null if the stream is not a serialized <see cref="ModelData"/>.</returns>
        /// <remarks>
        /// </remarks>
        public static ModelData Load(Stream stream)
        {
            var serializer = GetSerializer(stream, SerializerMode.Read);
            try
            {
                return serializer.Load<ModelData>();
            }
            catch (InvalidChunkException chunkException)
            {
                // If we have an exception of the magiccode, just return null
                if (chunkException.ExpectedChunkId == OdysseyIdentifier)
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        /// Saves this <see cref="ModelData"/> instance to the specified file.
        /// </summary>
        /// <param name="fileName">The output filename.</param>
        public void Save(string fileName)
        {
            using (var stream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                Save(stream);
        }

        /// <summary>
        /// Saves this <see cref="ModelData"/> instance to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            var serializer = GetSerializer(stream, SerializerMode.Write);
            serializer.Save(this);
        }

        private static BinarySerializer GetSerializer(Stream stream, SerializerMode mode)
        {
            var serializer = new BinarySerializer(stream, mode, Encoding.BigEndianUnicode)
            {
                ArrayLengthType = ArrayLengthType.Int
            };
            return serializer;
        }
    }
}
