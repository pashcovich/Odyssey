using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Talos.Nodes;
using SharpDX;

namespace Odyssey.Talos.Initializers
{
    internal class LightInitializer : Initializer<LightNode>
    {

        public LightInitializer()
            : base(new[] { 
            EngineReference.LightPointPS,
            EngineReference.LightPointVS, EngineReference.LightPointMatrixProjection, EngineReference.LightDirection})
        {
        }

        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var effect = initializer.Effect;
            InitializerParameters parameters = new InitializerParameters(-1, initializer.Technique, services, StaticSelector);
            var data = from metaData in effect.MetaData
                where metaData.Key == Param.Properties.LightId
                select Int32.Parse(metaData.Value);

            ILightService lightService = services.GetService<ILightService>();

            foreach (int lightId in data)
                initializer.Initialize(this, lightService[lightId], parameters);
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return cb.ContainsMetadata(Param.Properties.LightId);
        }

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent,LightNode lightNode, int parameterIndex, EngineReference reference, InitializerParameters initializerParameters)
        {
            string lightIdValue = cbParent.Get(Param.Properties.LightId);
            int lightId = Int32.Parse(lightIdValue);

            IEnumerable<IParameter> parameter = CreateParameter(lightNode, lightId, parameterIndex, reference);
            return parameter;
        }

        IEnumerable<IParameter> CreateParameter(LightNode lightNode, int lightId, int parameterIndex, EngineReference reference)
        {

            switch (reference)
            {
                case EngineReference.LightPointVS:
                    throw new NotImplementedException();

                case EngineReference.LightPointPS:
                    return Convert(parameterIndex, (PointLightNode)lightNode);

                case EngineReference.LightDirection:
                    return new[] {new Float3Parameter(parameterIndex, "lightdir", () => new Vector3(-1, 0, -1))};

                default:
                    throw new InvalidOperationException(string.Format("[{0}]: camera parameter not valid", reference));
            }
        }

        static IEnumerable<IParameter> Convert(int parameterIndex, PointLightNode lightNode)
        {
            var cLight = lightNode.LightComponent;

            List<IParameter> parameterCollection = new List<IParameter>
            {
                new Float4Parameter(parameterIndex, Param.Light.Params,
                    () => new Vector4(cLight.Intensity, cLight.Range, 0, 0)),
                new Float4Parameter(parameterIndex, Param.Light.Diffuse, () => cLight.Diffuse.ToVector4()),
                new Float3Parameter(parameterIndex, Param.Light.Position, () => lightNode.WorldPosition)
            };
            return parameterCollection;
        }

    }
}
