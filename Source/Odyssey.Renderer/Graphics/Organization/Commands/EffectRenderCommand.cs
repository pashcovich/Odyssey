using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;
using Odyssey.Utilities.Logging;
using SharpDX;

namespace Odyssey.Graphics.Organization.Commands
{
    [DebuggerDisplay("{Type}[{Effect.Name}]: {entities.Count} items")]
    public class EffectRenderCommand : RenderCommand, IEffectRenderCommand
    {
        

        public EffectRenderCommand(IServiceRegistry services, Effect effect, ModelMeshCollection renderables, IEnumerable<IEntity> entities)
            : base(services, effect, entities)
        {
            Contract.Requires<ArgumentNullException>(renderables != null);
            
            
            Renderables = renderables;
            Name = string.Format("{0}[{1}]", Type, Effect.Name);
        }

        public override void Initialize()
        {
            //Effect.AssembleBuffers(directX);
#if DEBUG
            CheckPreconditions();
#endif
        }

        void CheckPreconditions()
        {
            bool result = true;
            foreach (TextureMapping tm in Effect[ShaderType.Pixel].SelectTextures().Where(tm => tm.Texture == null))
            {
                LogEvent.Engine.Error("{0} is null [{1}.{2}].", tm.Description.Key, Effect[ShaderType.Pixel].Name,
                    tm.Description.Texture);
                result = false;
            }
            if (!result)
                throw new InvalidOperationException(string.Format("[{0}]: preconditions failed.", Name));
        }

        public override void PreRender()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            device.InputAssembler.InputLayout = Effect.InputLayout;

            foreach (TextureMapping tm in Effect[ShaderType.Pixel].SelectTextures(UpdateType.SceneStatic))
            {
                // TODO Update SamplerState assignment
                device.SetPixelShaderSampler(tm.Description.SamplerIndex, device.SamplerStates.Default);
                device.SetPixelShaderShaderResourceView(tm.Description.Index, tm.Texture.ShaderResourceView[ViewType.Full, 0,0]);
            }

            foreach (Shader shader in Effect)
                shader.Apply(Effect.Name, UpdateType.SceneStatic);

        }

        public override void PostRender()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            if (Effect[ShaderType.Pixel].HasTextures())
                foreach (TextureMapping tm in Effect[ShaderType.Pixel].SelectTextures(UpdateType.SceneStatic))
                {
                    device.SetPixelShaderShaderResourceView(tm.Description.Index, null);
                    device.SetPixelShaderSampler(tm.Description.SamplerIndex, null);
                }
        }

        public override void Render()
        {
            DirectXDevice device = DeviceService.DirectXDevice;
            device.CurrentEffect = Effect;
            foreach (Shader shader in Effect)
                shader.Apply(Effect.Name, UpdateType.SceneFrame);

            foreach (IEntity entity in Entities)
            {
                if (!entity.IsEnabled)
                    continue;
                
                foreach (Shader shader in Effect)
                {
                    shader.Apply(Effect.Name, entity.Id, UpdateType.InstanceStatic);
                    shader.Apply(Effect.Name, entity.Id, UpdateType.InstanceFrame);
                }

                foreach (ModelMesh mesh in Renderables)
                {
                    mesh.Draw(device);
                }
            }
        }
    }
}