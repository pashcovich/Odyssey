using System;
using System.Collections.Generic;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics.Models;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public partial class Designer
    {
        private readonly IServiceRegistry services;
        private Model model;
        private readonly List<ShapeMeshDescription> shapes;
        private readonly DirectXDevice device;
        
        public Model Result
        {
            get
            {
                if (model == null)
                    throw new InvalidOperationException("Result is not yet available, call Designer.End before accessing it");
                else return model;
            }
        }

        public Matrix Transform { get; set; }
        public IColorResource Color { get; set; }
        public ModelOperation ModelOperations { get; set; }

        public Designer(IServiceRegistry services)
        {
            this.services = services;
            device = services.GetService<IGraphicsDeviceService>().DirectXDevice;
            shapes = new List<ShapeMeshDescription>();
            ModelOperations = ModelOperation.None;
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
            
            model = GeometricPrimitive<VertexPositionNormalTexture>.New(device, "ShapeMesh", vertexList.ToArray(), indexList.ToArray(), modelOperations: ModelOperations);
        }

    }
}
