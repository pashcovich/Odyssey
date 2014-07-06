using System;
using System.IO;
using Odyssey.Content;
using Odyssey.Engine;
using SharpDX;

namespace Odyssey.Graphics.Models
{
    [ContentReader(typeof(ModelContentReader))]
    public class Model : Component
    {
        public ModelMeshCollection Meshes;
        public PropertyCollection Properties;

        private Model(string name)
        {
            Name = name;
            Meshes = new ModelMeshCollection();
            Properties = new PropertyCollection();
        }

        internal Model() : this("Undefined")
        { }

        internal Model(string name, PrimitiveType primitiveType, Buffer verteBuffer, VertexInputLayout layout, Buffer indexBuffer) : this (name)
        {
            ModelMesh modelMesh = ToDispose(new ModelMesh(name, primitiveType, verteBuffer, layout, indexBuffer));
            Meshes.Add(modelMesh);
        }

        public static Model Load(DirectXDevice graphicsDevice, Stream stream)
        {
            using (var serializer = new ModelReader(graphicsDevice, stream))
            {
                return serializer.ReadModel();
            }
        }

    }
}
