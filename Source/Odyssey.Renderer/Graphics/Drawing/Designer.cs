using System;
using System.Collections.Generic;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Geometry.Extensions;
using Odyssey.Graphics.Models;
using SharpDX.Direct3D11;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public partial class Designer
    {
        private readonly IServiceRegistry services;
        private readonly List<ShapeMeshDescription> shapes;
        private readonly DirectXDevice device;
        private VertexPositionColor[] vertices;
        private int[] indices;
        
        public Model Result
        {
            get
            {
                if (vertices == null)
                    throw new InvalidOperationException("Result is not yet available, call Designer.End before accessing it");
                else
                    return GeometricPrimitive<VertexPositionNormalTexture>.New(device, "ShapeMesh", vertices, indices, 
                        usage: ResourceUsage, modelOperations: ModelOperations);
            }
        }

        public Matrix Transform { get; set; }
        public IColorResource Color { get; set; }
        public ModelOperation ModelOperations { get; set; }
        public ResourceUsage ResourceUsage { get; set; }

        public Designer(IServiceRegistry services)
        {
            this.services = services;
            device = services.GetService<IGraphicsDeviceService>().DirectXDevice;
            shapes = new List<ShapeMeshDescription>();
            ModelOperations = ModelOperation.None;
            ResourceUsage = ResourceUsage.Default;
        }

        public void Begin()
        {
            Transform = Matrix.Identity;
            Color = new SolidColor("Default", Color4.White);
            shapes.Clear();
        }

        public void End()
        {
            Build();
        }

        void Build()
        {
            var vertexList = new List<VertexPositionColor>();
            var indexList = new List<int>();

            int index = 0;
            foreach (var shape in shapes)
            {
                vertexList.AddRange(shape.Vertices);
                int[] indexArray = shape.Indices;
                for (int i = 0; i < indexArray.Length; i++)
                    indexArray[i] += index;
                indexList.AddRange(indexArray);
                index += shape.Vertices.Length;
            }

            vertices = vertexList.ToArray();
            indices = indexList.ToArray();
        }

        public VertexPositionColor[] AccessData(out int[] indices)
        {
            indices = this.indices;
            return vertices;
        }

        static Vector3[] CheckTransform(Matrix transform, Vector3[] vertices)
        {
            if (!transform.IsIdentity)
            {
                for (var i = 0; i < vertices.Length; i++)
                {
                    var vertex = vertices[i];
                    vertices[i] = Vector3.Transform(vertex, transform).ToVector3();
                }
            }
            return vertices;
        }

    }
}
