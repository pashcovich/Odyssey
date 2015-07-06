using System;
using System.Linq;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Engine;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Models
{
    public class ModelMesh : ComponentBase, IModelMesh, IDisposable
    {
        internal delegate void DrawFunctionDelegate(DirectXDevice device);
        public BoundingSphere BoundingSphere;

        internal VertexBufferBindingCollection VertexBuffers;
        internal BufferCollection IndexBuffers;
        internal DrawFunctionDelegate DrawFunction;

        public ModelMeshPartCollection MeshParts;
        public PropertyCollection Properties;

        internal ModelMesh() : this("Undefined")
        {
            DrawFunction = DrawIndexed;
        }

        private ModelMesh(string name)
        {
            Name = name;
            MeshParts = new ModelMeshPartCollection();
            Properties = new PropertyCollection();
            VertexBuffers = new VertexBufferBindingCollection();
            IndexBuffers = new BufferCollection();
        }

        internal ModelMesh(string name, PrimitiveType primitiveType, Buffer vertexBuffer, VertexInputLayout layout, Buffer indexBuffer) : this(name)
        {
            var vertexBufferBinding = new VertexBufferBinding {Buffer = vertexBuffer, Layout = layout};
            VertexBuffers.Add(vertexBufferBinding);

            var meshPart = new ModelMeshPart()
                {
                    Name = Name,
                    VertexBuffer = new ModelBufferRange<VertexBufferBinding> { 
                        Count = vertexBuffer.ElementCount, 
                        Resource = vertexBufferBinding,
                        Start = 0},
                    ParentMesh = this,
                    PrimitiveType = primitiveType,
                };
            if (indexBuffer != null)
            {
                IndexBuffers.Add(indexBuffer);
                meshPart.IndexBuffer = new ModelBufferRange<Buffer>
                {
                    Count = indexBuffer.ElementCount,
                    Resource = indexBuffer,
                    Start = 0
                };
                meshPart.IsIndex32Bit = indexBuffer.ElementSize == 4;
                DrawFunction = DrawIndexed;
            }
            else
                DrawFunction = DrawUnindexed;
            
            MeshParts.Add(meshPart);
        }

        /// <summary>
        /// Iterator on each <see cref="ModelMeshPart"/>.
        /// </summary>
        /// <param name="meshPartFunction">The mesh part function.</param>
        public void ForEach(Action<ModelMeshPart> meshPartFunction)
        {
            int meshPartCount = MeshParts.Count;
            for (int i = 0; i < meshPartCount; i++)
            {
                meshPartFunction(MeshParts[i]);
            }
        }

        public void Draw(DirectXDevice device)
        {
            DrawFunction(device);
        }

        public void DrawUnindexed(DirectXDevice device)
        {
            foreach (ModelMeshPart part in MeshParts)
            {
                part.DrawUnindexed(device);
            }
        }

        public void DrawIndexed(DirectXDevice device)
        {
            foreach (ModelMeshPart part in MeshParts)
            {
                part.DrawIndexed(device);
            }
        }

        public void DrawIndexedInstanced(DirectXDevice device, int instanceCount)
        {
            foreach (ModelMeshPart part in MeshParts)
            {
                part.DrawIndexedInstanced(device, instanceCount);
            }
        }

        public void UpdateVertexBuffer<TVertex>(int index, TVertex[] data)
            where TVertex : struct 
        {
            VertexBuffers[index].Buffer.SetData(data);
        }

        public void UpdateIndexBuffer(int index, int[] indices)
        {
            if (indices.Length < 0xFFFF)
            {
                ushort[] shortIndices = indices.Select(i => (ushort)i).ToArray();
                IndexBuffers[index].SetData(shortIndices);
                MeshParts[index].IsIndex32Bit = false;
            }
            else
            {
                IndexBuffers[index].SetData(indices);
                MeshParts[index].IsIndex32Bit = true;
            }

            MeshParts[index].IndexBuffer.Resource = IndexBuffers[index];
            MeshParts[index].IndexBuffer.Count = indices.Length;
        }

        protected void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (ModelMeshPart part in MeshParts)
                {
                    if (part.VertexBuffer.Resource.Buffer != null)
                    {
                        part.VertexBuffer.Resource.Buffer.Dispose();
                        part.VertexBuffer.Resource.Buffer = null;
                    }
                    if (part.IndexBuffer.Resource != null)
                    {
                        part.IndexBuffer.Resource.Dispose();
                        part.IndexBuffer.Resource = null;
                    }
                }
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }

        ~ModelMesh()
        {
            Dispose(false);
        }
    }
}
