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
    public class InstancingRenderCommand : EffectRenderCommand
    {
        private Buffer instanceBuffer;
        private int elementSize;

        public InstancingRenderCommand(IServiceRegistry services, Effect effect, ModelMeshCollection renderables, IEnumerable<IEntity> entities) 
            : base(services, effect, renderables, entities)
        {
        }

        public override void Initialize()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            elementSize =
                Effect[ShaderType.Vertex].SelectBuffers(Effect.Name, Entities.First().Id, UpdateType.InstanceFrame).First().ElementSize;
            Effect.UpdateBuffers(UpdateType.InstanceFrame);
            var data = AggregateIntanceData();

            instanceBuffer = ToDispose(Buffer.Vertex.New(device,  data, ResourceUsage.Default));
            instanceBuffer.DebugName = string.Format("{0}_{1}", "IB", Renderables[0].Name);
        }

        Vector4[] AggregateIntanceData()
        {
            return (from e in Entities
                    from cb in Effect[ShaderType.Vertex].SelectBuffers(Effect.Name, e.Id, UpdateType.InstanceFrame)
                    select cb).SelectMany(cb => cb.Data).ToArray();
        }

        public override void Render()
        {
            DirectXDevice device = DeviceService.DirectXDevice;

            foreach (Shader shader in Effect)
                shader.Apply(Effect.Name, UpdateType.SceneFrame);

            var updatedData = AggregateIntanceData();
            instanceBuffer.SetData(device, updatedData);

            foreach (ModelMesh modelMesh in Renderables)
            {
                foreach (ConstantBuffer cb in Effect[ShaderType.Vertex].SelectBuffers(Effect.Name, UpdateType.InstanceFrame))
                    device.SetVertexBuffer(cb.Index, instanceBuffer, elementSize);
                modelMesh.DrawIndexedInstanced(device, EntityCount);
            }
        }

    }
}
