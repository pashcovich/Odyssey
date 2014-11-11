#region License

// Copyright © 2013-2014 Iter Astris - Adalberto L. Simeone
// Web: http://www.iterastris.uk E-mail: adal@iterastris.uk
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D;

#endregion

namespace Odyssey.Graphics.Models
{
    public class CylinderMesh
    {
        private readonly float diameter;
        private readonly float height;
        private readonly int tessellationH;
        private readonly int tessellationV;
        private readonly Vector2 tileFactor;

        public CylinderMesh(float diameter, float height, int tessellationH, int tessellationV, float tileX, float tileY)
        {
            this.diameter = diameter;
            this.height = height;
            this.tessellationH = tessellationH;
            this.tessellationV = tessellationV;
            tileFactor = new Vector2(tileX, tileY);
        }

        public void GenerateMesh(out VertexPositionNormalTexture[] vertices, out int[] indices)
        {
            var mb = new MeshBuilder();
            float deltaV = height / tessellationV;
            for (int i = 0; i <= tessellationV; i++)
            {
                Vector3 p = Vector3.Up * deltaV * i - height / 2 * Vector3.Up;
                float v = (float)i / tessellationV;
                mb.BuildRing(tessellationH, p, diameter / 2, v, i > 0);
            }

            mb.BuildCap(tessellationH, -height / 2 * Vector3.Up, diameter / 2, true);
            mb.BuildCap(tessellationH, Vector3.Up * height / 2, diameter / 2, false);

            vertices = mb.GetVertices();
            indices = mb.GetIndices();
        }

        /// <summary>
        ///     Creates a cone primitive.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="height">The height.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="tessellationH">The horizontal tessellation.</param>
        /// <param name="tessellationV">The vertical tessellation.</param>
        /// <param name="tileX">Horizontal texture coordinate scale factor.</param>
        /// <param name="tileY">Vertical texture coordinate scale factor.</param>
        /// <param name="modelOperations">Operations to perform on the model.</param>
        /// <returns>A cone primitive.</returns>
        public static Model New(DirectXDevice device,
            float diameter, float height,
            int tessellationH = 8, int tessellationV = 8,
            float tileX = 1.0f, float tileY = 1.0f,
            ModelOperation modelOperations = ModelOperation.None)
        {
            var cylinder = new CylinderMesh(diameter, height, tessellationH, tessellationV, tileX, tileY);
            VertexPositionNormalTexture[] vertices;
            int[] indices;
            cylinder.GenerateMesh(out vertices, out indices);

            return GeometricPrimitive.New(device, "Cone", vertices, indices, PrimitiveTopology.TriangleList, modelOperations);
        }
    }
}