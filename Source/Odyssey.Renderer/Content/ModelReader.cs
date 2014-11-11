using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Models;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Odyssey.Serialization;
using Buffer = Odyssey.Graphics.Buffer;
using Encoding = System.Text.Encoding;
using VertexBufferBinding = Odyssey.Graphics.Models.VertexBufferBinding;

namespace Odyssey.Content
{
    public class ModelReader : BinarySerializer
    {
        private DataPointer sharedPtr;

        public ModelReader(DirectXDevice device, Stream stream)
            : base(stream, SerializerMode.Read, Encoding.BigEndianUnicode)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            Contract.Requires<ArgumentNullException>(stream != null, "stream");

            Device = device;
            ArrayLengthType = ArrayLengthType.Int;
        }

        internal void AllocateSharedMemory(int size)
        {
            sharedPtr = new DataPointer(SharpDX.Utilities.AllocateMemory(size), size);
            ToDispose(sharedPtr.Pointer);
        }

        internal IntPtr SharedMemoryPointer
        {
            get
            {
                return sharedPtr.Pointer;
            }
        }

        protected Model Model;

        protected ModelMesh CurrentMesh;

        protected readonly DirectXDevice Device;

        protected virtual Model CreateModel()
        {
            return new Model();
        }

        protected virtual ModelMesh CreateModelMesh()
        {
            return new ModelMesh();
        }

        protected virtual ModelMeshPart CreateModelMeshPart()
        {
            return new ModelMeshPart();
        }

        protected virtual ModelMeshCollection CreateModelMeshCollection(int capacity)
        {
            return new ModelMeshCollection(capacity);
        }

        protected virtual ModelMeshPartCollection CreateModelMeshPartCollection(int capacity)
        {
            return new ModelMeshPartCollection(capacity);
        }

        protected virtual VertexBufferBindingCollection CreateVertexBufferBindingCollection(int capacity)
        {
            return new VertexBufferBindingCollection(capacity);
        }

        protected virtual PropertyCollection CreatePropertyCollection(int capacity)
        {
            return new PropertyCollection(capacity);
        }

        protected virtual BufferCollection CreateBufferCollection(int capacity)
        {
            return new BufferCollection(capacity);
        }

        protected virtual VertexBufferBinding CreateVertexBufferBinding()
        {
            return new VertexBufferBinding();
        }

        public Model ReadModel()
        {
            Model = CreateModel();
            var model = Model;
            ReadModel(ref model);
            return model;
        }

        protected virtual void ReadModel(ref Model model)
        {
            // Starts the whole ModelData by the magiccode "TKMD"
            // If the serializer don't find the TKMD, It will throw an
            // exception that will be catched by Load method.
            
            BeginChunk(ModelData.OdysseyIdentifier);

            // Writes the version
            int version = Reader.ReadInt32();
            if (version != ModelData.Version)
            {
                throw new NotSupportedException(string.Format("ModelData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, ModelData.Version));
            }

            // Allocated the shared memory used to load this Model
            AllocateSharedMemory(Reader.ReadInt32());

            // Mesh section
            BeginChunk("MESH");
            ReadMeshes(ref model.Meshes);
            EndChunk();

            // Close TKMD section
            EndChunk();
        }

        public delegate PropertyKey NameToPropertyKeyDelegate(string name);

        protected virtual void ReadProperties(ref PropertyCollection properties)
        {
            ReadProperties(ref properties, name => new PropertyKey(name));
        }

        protected virtual void ReadProperties(ref PropertyCollection properties, NameToPropertyKeyDelegate nameToKey)
        {
            if (nameToKey == null)
            {
                throw new ArgumentNullException("nameToKey");
            }

            int count = Reader.ReadInt32();

            if (properties == null)
            {
                properties = CreatePropertyCollection(count);
            }

            for (int i = 0; i < count; i++)
            {
                string name = null;
                object value = null;
                Serialize(ref name);
                SerializeDynamic(ref value, SerializeFlags.Nullable); ;

                var key = nameToKey(name);
                if (key == null)
                {
                    throw new InvalidOperationException(string.Format("Cannot convert property name [{0}] to null key", name));
                }

                properties[key] = value;
            }
        }


        protected virtual void ReadMeshes(ref ModelMeshCollection meshes)
        {
            ReadList(ref meshes, CreateModelMeshCollection, CreateModelMesh, ReadMesh);
        }

        protected virtual void ReadVertexBuffers(ref VertexBufferBindingCollection vertices)
        {
            ReadList(ref vertices, CreateVertexBufferBindingCollection, CreateVertexBufferBinding, ReadVertexBuffer);
        }

        protected virtual void ReadMeshParts(ref ModelMeshPartCollection meshParts)
        {
            ReadList(ref meshParts, CreateModelMeshPartCollection, CreateModelMeshPart, ReadMeshPart);
        }

        protected virtual void ReadIndexBuffers(ref BufferCollection list)
        {
            int count = Reader.ReadInt32();
            list = CreateBufferCollection(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(ReadIndexBuffer());
            }
        }

        protected virtual void ReadMesh(ref ModelMesh mesh)
        {
            CurrentMesh = mesh;
            string meshName = null;
            Serialize(ref meshName, false, SerializeFlags.Nullable);
            mesh.Name = meshName;

            // Read the bounding sphere
            Serialize(ref mesh.BoundingSphere);

            ReadVertexBuffers(ref mesh.VertexBuffers);
            ReadIndexBuffers(ref mesh.IndexBuffers);

            if (mesh.IndexBuffers.Count == 0)
                mesh.DrawFunction = mesh.DrawUnindexed;

            ReadMeshParts(ref mesh.MeshParts);

            ReadProperties(ref mesh.Properties);
            CurrentMesh = null;
        }

        protected virtual void ReadMeshPart(ref ModelMeshPart meshPart)
        {
            // Set the Parent mesh for the current ModelMeshPart.
            meshPart.ParentMesh = CurrentMesh;

            // IndexBuffer
            var indexBufferRange = default(ModelData.BufferRange);
            indexBufferRange.Serialize(this);
            if (CurrentMesh.IndexBuffers.Count > 0)
            {
                meshPart.IndexBuffer = GetFromList(indexBufferRange, CurrentMesh.IndexBuffers);
                meshPart.IsIndex32Bit = meshPart.IndexBuffer.Resource.ElementSize == 4;
            }

            // VertexBuffer
            var vertexBufferRange = default(ModelData.BufferRange);
            vertexBufferRange.Serialize(this);
            meshPart.VertexBuffer = GetFromList(vertexBufferRange, CurrentMesh.VertexBuffers);

            // Properties
            PrimitiveTopology primitiveTopology = PrimitiveTopology.Undefined;
            SerializeEnum(ref primitiveTopology);
            meshPart.PrimitiveType = PrimitiveType.New(primitiveTopology);
            ReadProperties(ref meshPart.Properties);
        }

        protected virtual void ReadVertexBuffer(ref VertexBufferBinding vertexBufferBinding)
        {
            // Read the number of vertices
            int count = Reader.ReadInt32();

            // Read vertex elements
            int vertexElementCount = Reader.ReadInt32();
            var elements = new VertexElement[vertexElementCount];
            for (int i = 0; i < vertexElementCount; i++)
            {
                elements[i].Serialize(this);
            }
            vertexBufferBinding.Layout = VertexInputLayout.New(0, elements);

            // Read Vertex Buffer
            int sizeInBytes = Reader.ReadInt32();
            SerializeMemoryRegion(SharedMemoryPointer, sizeInBytes);
            vertexBufferBinding.Buffer = Buffer.New(Device, new DataPointer(SharedMemoryPointer, sizeInBytes), sizeInBytes / count, BufferFlags.VertexBuffer, ResourceUsage.Immutable);
        }

        protected virtual Buffer ReadIndexBuffer()
        {
            int indexCount = Reader.ReadInt32();
            int sizeInBytes = Reader.ReadInt32();
            SerializeMemoryRegion(SharedMemoryPointer, sizeInBytes);
            return Buffer.New(Device, new DataPointer(SharedMemoryPointer, sizeInBytes), sizeInBytes / indexCount, BufferFlags.IndexBuffer, ResourceUsage.Immutable);
        }

        protected delegate TList CreateListDelegate<out TList, TItem>(int list) where TList : List<TItem>;

        protected delegate T CreateItemDelegate<out T>();

        protected delegate void ReadItemDelegate<T>(ref T item);

        protected virtual TList ReadList<TList, TItem>(ref TList list, CreateListDelegate<TList, TItem> listCreate, CreateItemDelegate<TItem> itemCreate, ReadItemDelegate<TItem> itemReader) where TList : List<TItem>
        {
            int count = Reader.ReadInt32();
            list = listCreate(count);
            for (int i = 0; i < count; i++)
            {
                var item = itemCreate();
                itemReader(ref item);
                list.Add(item);
            }
            return list;
        }

        private ModelBufferRange<T> GetFromList<T>(ModelData.BufferRange range, IList<T> list)
        {
            var index = range.Slot;
            if (index >= list.Count)
            {
                throw new InvalidOperationException(string.Format("Invalid slot [{0}] for {1} (Max: {2})", index, typeof(T).Name, list.Count));
            }
            return new ModelBufferRange<T> { Resource = list[index], Count = range.Count, Start = range.Start};
        }
    }
}
