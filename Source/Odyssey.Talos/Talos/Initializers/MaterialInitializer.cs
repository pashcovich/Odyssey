using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Talos.Nodes;
using SharpDX;

namespace Odyssey.Talos.Initializers
{
    public class MaterialInitializer : Initializer<IMaterial>
    {

        public MaterialInitializer()
            : base(new[] { EngineReference.Material, EngineReference.MaterialDiffuse })
        {
        }

        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var effect = initializer.Effect;
            
            var scene = services.GetService<IScene>();
            var data = from e in scene.Entities
                       where e.ContainsComponent<MaterialComponent>()
                       let techniqueComponents = e.Components.OfType<ITechniqueComponent>()
                from techniqueRange in techniqueComponents
                from technique in techniqueRange.Techniques
                where technique.ActiveTechniqueId == effect.Name
                group e.Id by e.GetComponent<MaterialComponent>()
                into materialEntities
                select new {Material = materialEntities.Key, Entities = materialEntities.ToArray()};

            foreach (var kvp in data)
            {
                foreach (var entityId in kvp.Entities)
                {
                    InitializerParameters parameters = new InitializerParameters(entityId, initializer.Technique, services, InstanceSelector);
                    initializer.Initialize(this, (IMaterial) kvp.Material, parameters);
                }
            }
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return cb.ContainsMetadata(Param.Properties.Material);
        }

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, IMaterial material, int parameterIndex, EngineReference reference, InitializerParameters initializerParameterse)
        {
            return CreateParameter(material, parameterIndex, reference);
        }

        static IEnumerable<IParameter> CreateParameter(IMaterial material, int parameterIndex, EngineReference reference)
        {

            switch (reference)
            {

                case EngineReference.MaterialDiffuse:
                    return new [] { new Float4Parameter(parameterIndex,Param.Material.Diffuse, () => material.Diffuse)};

                case EngineReference.Material:
                    return Convert(parameterIndex, material);

                default:
                    throw new ArgumentException(string.Format("EngineReference: {0} not valid.", reference));
            }
        }

        static IEnumerable<IParameter> Convert(int parameterIndex, IMaterial material)
        {
         
            List<IParameter> parameterCollection = new List<IParameter>
            {
                new Float4Parameter(parameterIndex, Param.Material.Coefficients, () =>
                    new Vector4(material.AmbientCoefficient,
                        material.DiffuseCoefficient,
                        material.SpecularCoefficient,
                        material.SpecularPower)),
                new Float4Parameter(parameterIndex, Param.Material.Ambient, () => material.Ambient),
                new Float4Parameter(parameterIndex, Param.Material.Diffuse, () => material.Diffuse),
                new Float4Parameter(parameterIndex, Param.Material.Specular, () => material.Specular)
            };
            return parameterCollection;
        }

    }
}
