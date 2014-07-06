using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Odyssey.Content;
using Odyssey.Engine;
using SharpDX;

namespace Odyssey.Graphics.Models
{
    /// <summary>
    /// Represents a batch of geometry information to submit to the graphics device during rendering. 
    /// Each <see cref="ModelMeshPart"/> is a subdivision of a <see cref="ModelMesh"/> object. The <see cref="ModelMesh"/> class is split into multiple <see cref="ModelMeshPart"/> objects, typically based on material information. 
    /// See remarks for differences with XNA.
    /// </summary>
    /// <remarks>
    /// Unlike XNA, a <see cref="ModelMeshPart"/> is not bound to a specific Effect. The effect must have been setup prior calling the <see cref="DrawUnindexed"/> method on this instance.
    /// The <see cref="DrawUnindexed"/> method is only responsible to setup the VertexBuffer, IndexBuffer and call the appropriate <see cref="GraphicsDevice.DrawIndexed"/> method on the <see cref="GraphicsDevice"/>.
    /// </remarks>
    public class ModelMeshPart : ComponentBase
    {
        /// <summary>
        /// The parent mesh.
        /// </summary>
        public ModelMesh ParentMesh;

        public PrimitiveType PrimitiveType;
        public bool IsIndex32Bit;

        /// <summary>
        /// The index buffer range for this mesh part.
        /// </summary>
        public ModelBufferRange<Buffer> IndexBuffer;

        /// <summary>
        /// The vertex buffer range for this mesh part.
        /// </summary>
        public ModelBufferRange<VertexBufferBinding> VertexBuffer;

        /// <summary>
        /// The attributes for this mesh part.
        /// </summary>
        public PropertyCollection Properties;

        /// <summary>
        /// Draws this <see cref="ModelMeshPart"/>. See remarks for difference with XNA.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <remarks>
        /// Unlike XNA, a <see cref="ModelMeshPart"/> is not bound to a specific Effect. The effect must have been setup prior calling this method.
        /// This method is only responsible to setup the VertexBuffer, IndexBuffer and call the appropriate <see cref="DirectxDevice.DrawIndexed"/> method on the <see cref="GraphicsDevice"/>.
        /// </remarks>
        public void DrawUnindexed(DirectXDevice device)
        {
            SetBuffers(device);

            // Finally Draw this mesh
            device.Draw(PrimitiveType, VertexBuffer.Count);
        }

        public void DrawIndexed(DirectXDevice device)
        {
            SetBuffers(device);

            // Draw this mesh
            device.DrawIndexed(PrimitiveType, IndexBuffer.Count);
        }

        public void DrawIndexedInstanced(DirectXDevice device, int instanceCount)
        {
            SetBuffers(device);

            // Draw this mesh
            device.DrawIndexedInstanced(PrimitiveType, IndexBuffer.Count, instanceCount);
        }

        void SetBuffers(DirectXDevice device)
        {
            // Setup the Vertex Buffer
            var vertexBuffer = VertexBuffer.Resource.Buffer;
            var elementSize = vertexBuffer.ElementSize;
            device.SetVertexBuffer(0, vertexBuffer, elementSize, VertexBuffer.Start == 0 ? 0 : VertexBuffer.Start * elementSize);

            // Setup the Vertex Buffer Input layout
            device.SetVertexInputLayout(VertexBuffer.Resource.Layout);

            // Setup the index Buffer
            var indexBuffer = IndexBuffer.Resource;
            device.SetIndexBuffer(indexBuffer, IsIndex32Bit, IndexBuffer.Start == 0 ? 0 : IndexBuffer.Start * indexBuffer.ElementSize);
        }
    }
}
