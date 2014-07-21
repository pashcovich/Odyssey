using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.Models;
using SharpDX;

namespace Odyssey.Graphics.Shapes
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

        public Designer(IServiceRegistry services)
        {
            this.services = services;
            device = services.GetService<IOdysseyDeviceService>().DirectXDevice;
            shapes = new List<ShapeMeshDescription>();
        }

        public void Begin()
        {
            shapes.Clear();
        }

        public void End()
        {
            Build();
        }

        void Build()
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            List<int> indexList = new List<int>();

            int index = 0;
            foreach (var shape in shapes)
            {
                vertexList.AddRange(shape.Vertices);
                int[] indexArray = shape.Indices;
                for (int i = 0; i < indexArray.Length; i++)
                    indexArray[i] += index;
                indexList.AddRange(indexArray);
                index += indexArray.Length;
            }
            
            model = GeometricPrimitive.New(device, "ShapeMesh", vertexList.ToArray(), indexList.ToArray());
        }

    }
}
