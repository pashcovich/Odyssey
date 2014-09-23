using System;
using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Graphics.Shaders;
using SharpDX;
using EngineReference = Odyssey.Graphics.Effects.EngineReference;

namespace Odyssey.Talos.Initializers
{
    public class ApplicationInitializer : Initializer<IGraphicsDeviceService>
    {
        public ApplicationInitializer(IServiceRegistry services)
            : base(services, Reference.Group.Application)
        {
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return true;
        }


        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var settings = services.GetService<IGraphicsDeviceService>();

            InitializerParameters parameters = new InitializerParameters(-1,initializer.Technique, services, StaticSelector);
            initializer.Initialize(this, settings, parameters);
        }

        private static readonly Dictionary<string, ParameterMethod> ReferenceActions = new Dictionary<string, ParameterMethod>
        {
            {
                Reference.Application.ViewportSize, (index, deviceService, parameters) => 
                new [] { new Float2Parameter(index, Param.Vectors.ViewportSize, () => new Vector2(deviceService.DirectXDevice.Viewport.Width, deviceService.DirectXDevice.Viewport.Height))}
            },
        };

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, IGraphicsDeviceService source,
            int parameterIndex, string reference, InitializerParameters initializerParameters)
        {
            if (!ReferenceActions.ContainsKey(reference))
                throw new ArgumentException(string.Format("{0}: {1} not valid.", GetType().Name, reference));

            return ReferenceActions[reference](parameterIndex, source, initializerParameters);
        }
    }
}
