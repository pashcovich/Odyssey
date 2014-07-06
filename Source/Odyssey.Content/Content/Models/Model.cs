using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Content.Meshes;
using Odyssey.Graphics;
using SharpDX.Serialization;
using SharpDX;

namespace Odyssey.Content.Models
{
    public class Model : Component, IDataSerializable, IInitializable, IByteSize
    {
        private static float Version = 0.5f;
        private readonly MeshDataCollection meshParts;
        private readonly MeshCollection meshCollection;

        public bool IsInited { get; private set; }

        internal MeshDataCollection MeshParts { get { return meshParts; } }
        public MeshCollection Meshes { get { return meshCollection; } }

        public Model() : base("Undefined")
        {
            meshParts = new MeshDataCollection();
            meshCollection = ToDispose(new MeshCollection());
        }

        public void Serialize(BinarySerializer serializer)
        {
            string name = Name;
            serializer.Serialize(ref Version);
            serializer.Serialize(ref name);
            serializer.SerializeThis(meshParts, SerializeFlags.Normal);

            if (serializer.Mode == SerializerMode.Read)
            {
                foreach (MeshData meshPart in meshParts)
                    meshPart.ParentMesh = this;
            }
        }

        public void LoadMeshData(IEnumerable<MeshData> collection)
        {
            meshParts.AddRange(collection);
            foreach (MeshData meshPart in meshParts)
                meshPart.ParentMesh = this;
        }

        public void Unload()
        {
            meshCollection.Dispose();
            IsInited = false;
        }

        public void Initialize(Engine.IDirectXProvider directX)
        {
            Contract.Assert(!meshParts.IsEmpty, "Warning: no mesh data found.");
            meshCollection.AddMeshRange(meshParts.ToMeshCollection());
            meshCollection.Initialize(directX);
            IsInited = true;
        }

        public int ByteSize
        {
            get { return meshParts.Sum(m => m.ByteSize); }
        }
    }
}
