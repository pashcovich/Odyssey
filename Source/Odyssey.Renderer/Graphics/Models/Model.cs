using System.IO;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Engine;

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

        internal Model() : this(string.Empty)
        { }

        internal Model(string name, PrimitiveType primitiveType, Buffer verteBuffer, VertexInputLayout layout, Buffer indexBuffer) : this (name)
        {
            ModelMesh modelMesh = ToDispose(new ModelMesh(name, primitiveType, verteBuffer, layout, indexBuffer));
            Meshes.Add(modelMesh);
        }

        public static Model Load(DirectXDevice graphicsDevice, Stream stream, string assetName)
        {
            using (var serializer = new ModelReader(graphicsDevice, stream))
            {
                Model model = serializer.ReadModel();
                if (string.IsNullOrEmpty(model.Name))
                    model.Name = assetName;
                model.RegisterResources();
                return model;
            }
        }

        void RegisterResources()
        {
            foreach (var modelMesh in Meshes)
                ToDispose(modelMesh);
        }
    }
}
