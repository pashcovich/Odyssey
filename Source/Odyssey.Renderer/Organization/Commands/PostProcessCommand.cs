using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Epos;
using SharpDX.Direct3D11;

namespace Odyssey.Organization.Commands
{
    [DebuggerDisplay("{Type}[{technique.Name}]")]
    public class PostProcessCommand : RenderToTextureCommandBase, ITechniqueRenderCommand
    {
        private readonly Technique technique;
        private readonly IEntity entity;
        private readonly ModelMesh quad;
        public Technique Technique { get { return technique; } }

        public PostProcessCommand(IServiceRegistry services, Technique technique, ModelMesh quad, IEntity entity, Texture2DDescription textureDescription = default (Texture2DDescription), OutputRule outputRule = OutputRule.NewRenderTarget) 
            : base(services, textureDescription, outputRule)
        {
            Contract.Requires<ArgumentNullException>(technique != null, "technique");
            Contract.Requires<ArgumentNullException>(quad!= null, "quad");
            Contract.Requires<ArgumentNullException>(entity != null, "entity");
            this.technique = technique;
            this.quad = quad;
            this.entity = entity;
        }

        public override void PreRender()
        {
            base.PreRender();

            DirectXDevice device = DeviceService.DirectXDevice;
            device.SetCurrentEffect(Technique);
            device.InputAssembler.InputLayout = Technique.InputLayout;
            device.SetPixelShaderSampler(0, device.SamplerStates.Default);
            var inputs = Inputs.ToArray();
            for (int i = 0; i < inputs.Length; i++)
            {
                var texture = inputs[i];
                device.SetPixelShaderShaderResourceView(i, texture.ShaderResourceView[ViewType.Full, 0, 0]);
            }

            foreach (Shader shader in Technique)
            {
                shader.UpdateBuffers(UpdateType.SceneStatic);
                shader.Apply(Technique.Name, UpdateType.SceneStatic);
            }
        }

        public override void Render()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            foreach (Shader shader in Technique)
                shader.Apply(Technique.Name, UpdateType.SceneFrame);

            foreach (Shader shader in Technique)
            {
                shader.Apply(Technique.Name, entity.Id, UpdateType.InstanceStatic);
                shader.Apply(Technique.Name, entity.Id, UpdateType.InstanceFrame);
            }

            quad.Draw(device);

            device.SetPixelShaderSampler(0, null);
                    
                    var inputs = Inputs.ToArray();
            for (int i = 0; i < inputs.Length; i++)
            {
                device.SetPixelShaderShaderResourceView(i, null);
            }
        }
    }
}
