using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics.Organization.Commands
{
    public class InstancingRenderCommand : TechniqueRenderCommand
    {
        private Buffer instanceBuffer;
        private int elementSize;

        public InstancingRenderCommand(IServiceRegistry services, Technique technique, ModelMeshCollection renderables, IEnumerable<IEntity> entities) 
            : base(services, technique, renderables, entities)
        {
        }

        public override void Initialize()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            elementSize =
                Technique[ShaderType.Vertex].SelectBuffers(Technique.Name, Entities.First().Id, UpdateType.InstanceFrame).First().ElementSize;
            Technique.UpdateBuffers(UpdateType.InstanceFrame);
            var data = AggregateIntanceData();

            instanceBuffer = ToDispose(Buffer.Vertex.New(device,  data, ResourceUsage.Default));
            instanceBuffer.DebugName = string.Format("{0}_{1}", "IB", Renderables[0].Name);
        }

        Vector4[] AggregateIntanceData()
        {
            return (from e in Entities
                    from cb in Technique[ShaderType.Vertex].SelectBuffers(Technique.Name, e.Id, UpdateType.InstanceFrame)
                    select cb).SelectMany(cb => cb.Data).ToArray();
        }

        public override void Render()
        {
            DirectXDevice device = DeviceService.DirectXDevice;

            foreach (Shader shader in Technique)
                shader.Apply(Technique.Name, UpdateType.SceneFrame);

            var updatedData = AggregateIntanceData();
            instanceBuffer.SetData(device, updatedData);

            foreach (ModelMesh modelMesh in Renderables)
            {
                foreach (ConstantBuffer cb in Technique[ShaderType.Vertex].SelectBuffers(Technique.Name, UpdateType.InstanceFrame))
                    device.SetVertexBuffer(cb.Index, instanceBuffer, elementSize);
                modelMesh.DrawIndexedInstanced(device, EntityCount);
            }
        }

    }
}
