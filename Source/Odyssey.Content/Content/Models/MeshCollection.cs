using System.Diagnostics.Contracts;
using Odyssey.Content.Meshes;
using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Content.Models
{
    public class MeshCollection : Component, IEnumerable<Mesh>
    {
        readonly List<Mesh> meshes;

        public int Count { get { return meshes.Count; } }

        public MeshCollection()
        {
            meshes = new List<Mesh>();
        }

        public MeshCollection(IEnumerable<Mesh> meshes) :this()
        {
            this.meshes.AddRange(ToDispose(meshes));
        }

        [Pure]
        public bool ContainsMesh(Mesh mesh)
        {
            return meshes.Contains(mesh);
        }

        public void AddMesh(Mesh mesh)
        {
            Contract.Requires<ArgumentNullException>(mesh!=null);
            Contract.Requires<ArgumentException>(!ContainsMesh(mesh));
            meshes.Add(ToDispose(mesh));
        }

        public void AddMeshRange(IEnumerable<Mesh> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            foreach (Mesh mesh in collection)
                AddMesh(mesh);
        }

        IEnumerator<Mesh> IEnumerable<Mesh>.GetEnumerator()
        {
            return meshes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return meshes.GetEnumerator();
        }

        public void Initialize(IDirectXProvider directX)
        {
            foreach (Mesh mesh in this)
            {
                mesh.Initialize(directX);
            }
        }

    }
}

