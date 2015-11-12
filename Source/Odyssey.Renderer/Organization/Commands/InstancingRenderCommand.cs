using System.Collections.Generic;
using System.Linq;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Epos;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = Odyssey.Graphics.Buffer;

namespace Odyssey.Organization.Commands
{
    public class InstancingRenderCommand : TechniqueRenderCommand
    {
        private Buffer instanceBuffer;
        private int elementSize;

        public InstancingRenderCommand(IServiceRegistry services, Technique technique, Model model, IEnumerable<IEntity> entities) 
            : base(services, technique, model, entities)
        {
        }

        public override void Initialize()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            elementSize =
                Technique[ShaderType.Vertex].SelectBuffers(Technique.Name, Entities.First().Id, CBUpdateType.InstanceFrame).First().ElementSize;
            Technique.UpdateBuffers(CBUpdateType.InstanceFrame);
            var data = AggregateIntanceData();

            instanceBuffer = ToDispose(Buffer.Vertex.New(device,  data, ResourceUsage.Default));
            instanceBuffer.DebugName = string.Format("{0}_{1}", "IB", Model.Name);
        }

        Vector4[] AggregateIntanceData()
        {
            return (from e in Entities
                    from cb in Technique[ShaderType.Vertex].SelectBuffers(Technique.Name, e.Id, CBUpdateType.InstanceFrame)
                    select cb).SelectMany(cb => cb.Data).ToArray();
        }

        public override void Render()
        {
            DirectXDevice device = DeviceService.DirectXDevice;

            foreach (Shader shader in Technique)
                shader.Apply(Technique.Name, CBUpdateType.SceneFrame);

            var updatedData = AggregateIntanceData();
            instanceBuffer.SetData(device, updatedData);

            foreach (ModelMesh modelMesh in Model.Meshes)
            {
                foreach (ConstantBuffer cb in Technique[ShaderType.Vertex].SelectBuffers(Technique.Name, CBUpdateType.InstanceFrame))
                    device.SetVertexBuffer(cb.Index, instanceBuffer, elementSize);
                modelMesh.DrawIndexedInstanced(device, EntityCount);
            }
        }

    }
}
