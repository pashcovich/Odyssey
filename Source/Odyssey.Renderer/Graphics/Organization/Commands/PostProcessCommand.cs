using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics.Organization.Commands
{
    [DebuggerDisplay("{Type}[{effect.Name}]")]
    public class PostProcessCommand : RenderToTextureCommandBase, IEffectRenderCommand
    {
        private readonly Effect effect;
        private readonly IEntity entity;
        private readonly ModelMesh quad;
        public Effect Effect { get { return effect; } }

        public PostProcessCommand(IServiceRegistry services, Effect effect, ModelMesh quad, IEntity entity, Texture2DDescription textureDescription = default (Texture2DDescription), TargetOutput output = TargetOutput.NewRenderTarget) 
            : base(services, textureDescription, output)
        {
            Contract.Requires<ArgumentNullException>(effect != null, "effect");
            Contract.Requires<ArgumentNullException>(quad!= null, "quad");
            Contract.Requires<ArgumentNullException>(entity != null, "entity");
            this.effect = effect;
            this.quad = quad;
            this.entity = entity;
            Inputs = new List<Texture>();
        }

        public override void PreRender()
        {
            base.PreRender();

            DirectXDevice device = DeviceService.DirectXDevice;
            device.SetCurrentEffect(Effect);
            device.InputAssembler.InputLayout = Effect.InputLayout;
            device.SetPixelShaderSampler(0, device.SamplerStates.Default);
            for (int i = 0; i < Inputs.Count; i++)
            {
                var texture = Inputs[i];
                device.SetPixelShaderShaderResourceView(i, texture.ShaderResourceView[ViewType.Full, 0, 0]);
            }

            foreach (Shader shader in Effect)
            {
                shader.UpdateBuffers(UpdateType.SceneStatic);
                shader.Apply(Effect.Name, UpdateType.SceneStatic);
            }
        }

        public override void Render()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            foreach (Shader shader in Effect)
                shader.Apply(Effect.Name, UpdateType.SceneFrame);

            foreach (Shader shader in Effect)
            {
                shader.Apply(Effect.Name, entity.Id, UpdateType.InstanceStatic);
                shader.Apply(Effect.Name, entity.Id, UpdateType.InstanceFrame);
            }

            quad.Draw(device);
        }


    }
}
