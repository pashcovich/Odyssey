using System;
using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using SharpDX;

namespace Odyssey.Talos.Initializers
{
    public class ApplicationInitializer : Initializer<IOdysseyDeviceService>
    {
        public ApplicationInitializer()
            : base(new[] {EngineReference.ApplicationCurrentViewportSize})
        {
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return true;
        }


        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var settings = services.GetService<IOdysseyDeviceService>();

            InitializerParameters parameters = new InitializerParameters(-1, initializer.Technique, services, StaticSelector);
            initializer.Initialize(this, settings, parameters);
        }

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, IOdysseyDeviceService source, int parameterIndex, EngineReference reference, InitializerParameters parameters)
        {
            var device = source.DirectXDevice;
            switch (reference)
            {
                case EngineReference.ApplicationCurrentViewportSize:
                    return new [] { new Float2Parameter(parameterIndex, Param.Vectors.ViewportSize, () => new Vector2(device.Viewport.Width, device.Viewport.Height))};

                default:
                    throw new ArgumentException(string.Format("{0}: {1} not valid.", GetType().Name, reference));
            }
        }
    }
}
