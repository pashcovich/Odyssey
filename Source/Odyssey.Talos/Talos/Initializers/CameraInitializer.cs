using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Talos.Nodes;

namespace Odyssey.Talos.Initializers
{
    internal class CameraInitializer: Initializer<IEntity>
    {

        public CameraInitializer()
            : base(new[] { 
            EngineReference.CameraVectorPosition,
            EngineReference.CameraMatrixView, EngineReference.CameraMatrixProjection})
        {
        }

        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var scene = services.GetService<IScene>();
            var effect = initializer.Effect;
            InitializerParameters parameters = new InitializerParameters(-1, initializer.Technique, services, StaticSelector);
            var data = from metaData in effect.MetaData
                       where metaData.Key == Param.Properties.CameraId
                       select Int32.Parse(metaData.Value);

            var cameras = (from e in scene.Entities
                where e.ContainsComponent<CameraComponent>()
                let cameraComponent = e.GetComponent<CameraComponent>()
                select new {Entity = e, CameraComponent = cameraComponent}).ToDictionary(kvp=> kvp.CameraComponent.Index, kvp => kvp.Entity);

            foreach (int cameraId in data)
                initializer.Initialize(this, cameras[cameraId], parameters);
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return cb.ContainsMetadata(Param.Properties.CameraId);
        }

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, IEntity entity, int parameterIndex, EngineReference reference, InitializerParameters initializerParameters)
        {
            string cameraIdValue = cbParent.Get(Param.Properties.CameraId);
            int cameraId = Int32.Parse(cameraIdValue);

            IEnumerable<IParameter> parameters = CreateParameter(entity, cameraId, parameterIndex, reference);
            return parameters;
        }

        IEnumerable<IParameter> CreateParameter(IEntity entity, int cameraId, int parameterIndex, EngineReference reference)
        {
            switch (reference)
            {
                case EngineReference.CameraVectorPosition:
                    return new[] { new Float3Parameter(parameterIndex, Param.Vectors.CameraPosition, () => entity.GetComponent<PositionComponent>().Position) };

                case EngineReference.CameraMatrixView:
                    return new[] { new MatrixParameter(parameterIndex, Param.Matrices.View, () => entity.GetComponent<CameraComponent>().View) };

                case EngineReference.CameraMatrixProjection:
                    return new[] { new MatrixParameter(parameterIndex, Param.Matrices.Projection, () => entity.GetComponent<CameraComponent>().Projection) };

                default:
                    throw new InvalidOperationException(string.Format("[{0}]: camera parameter not valid", reference));
            }
        }
    }
}
