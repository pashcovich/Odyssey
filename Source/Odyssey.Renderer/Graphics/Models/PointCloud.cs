using Odyssey.Engine;
using SharpDX.Direct3D;

namespace Odyssey.Graphics.Models
{
    public class PointCloud : IPrimitiveGenerator<VertexPositionNormalTexture>
    {
        private readonly int pointCount;

        public PointCloud(int pointCount)
        {
            this.pointCount = pointCount;
        }

        public static Model New(DirectXDevice device, int points, ModelOperation modelOperations = ModelOperation.None)
        {
            var pointCloud = new PointCloud(points);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            
            pointCloud.GenerateMesh(out vertices, out indices);
            return GeometricPrimitive.New(device, "PointCloud", vertices, indices, PrimitiveTopology.PointList,
                modelOperations);

        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            vertices = new VertexPositionNormalTexture[pointCount];
            indices = new int[0];
        }
    }
}
