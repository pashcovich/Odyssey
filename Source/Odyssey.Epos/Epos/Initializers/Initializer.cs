using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Mathematics;
using EngineReference = Odyssey.Engine.EngineReference;

namespace Odyssey.Epos.Initializers
{
    public abstract class Initializer<TSource> : IInitializer
    {
        protected delegate IEnumerable<IParameter> ParameterMethod(int index, TSource source, InitializerParameters parameters);

        protected static readonly SelectorDelegate DefaultSelector = cb => true;
        protected static readonly SelectorDelegate InstanceSelector = cb => cb.UpdateFrequency == UpdateType.InstanceFrame || cb.UpdateFrequency == UpdateType.InstanceStatic;
        protected static readonly SelectorDelegate StaticSelector = cb => cb.UpdateFrequency == UpdateType.SceneStatic || cb.UpdateFrequency == UpdateType.SceneFrame;
        private DirectXDevice device;

        /// <summary>
        /// Creates an Initializer responsible of a subset of <see cref="EngineReference"/> data initialization.
        /// </summary>
        /// <param name="referenceGroup">The type of <see cref="EngineReference"/> data.</param>
        protected Initializer(IServiceRegistry services, string referenceGroup)
        {
            var sRefs = services.GetService<IReferenceService>();
            AcceptedReferences = sRefs.Select(referenceGroup).ToArray();
        }

        public EngineReference[] AcceptedReferences { get; private set; }

        public virtual void Initialize(DirectXDevice device, TSource source, InitializerParameters parameters)
        {
            LogEvent.Engine.Info("[{0}, {1}] in {2}", GetType().Name, parameters.EntityId, parameters.Technique.Name);
            this.device = device;
            InitializeConstantBuffers(source, parameters);
        }

        public abstract void SetupInitialization(ShaderInitializer initializer);

        protected abstract IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, TSource source, int parameterIndex, string reference, InitializerParameters initializerParameters);

        protected abstract bool ValidateConstantBuffer(ConstantBufferDescription cb);

        private void InitializeConstantBuffers(TSource source, InitializerParameters parameters)
        {
            var technique = parameters.Technique;
            var referenceTable = from shaderObject in technique.Mapping.Shaders
                                 from cb in shaderObject.ConstantBuffers
                                 where parameters.Selector(cb) && ValidateConstantBuffer(cb)
                                 select cb;

            string effectName = technique.Name;
            foreach (var cbDesc in referenceTable)
            {
                if (!technique[cbDesc.ShaderType].HasConstantBuffer(cbDesc.Index, effectName, parameters.EntityId))
                    technique[cbDesc.ShaderType].AddConstantBuffer(parameters.EntityId, new ConstantBuffer(device, cbDesc, effectName));

                ConstantBuffer cb = technique[cbDesc.ShaderType].GetConstantBuffer(cbDesc.Index, effectName, parameters.EntityId);

                var validReferences = from kvp in cbDesc.References
                                      let reference = kvp.Value
                                      where AcceptedReferences.Any(r => string.Equals(r.Value, reference.Value))
                                      select kvp;

                foreach (var kvp in validReferences)
                {
                    var reference = kvp.Value;
                    if (cbDesc.UpdateFrequency != UpdateType.InstanceFrame && cbDesc.IsParsed(effectName, reference))
                        continue;

                    var effectParameters = CreateParameter(cbDesc, source, kvp.Key, reference.Value, parameters);

                    foreach (IParameter parameter in effectParameters)
                    {
                        cb.AddParameter(kvp.Key, parameter);
                    }
                    cbDesc.MarkAsParsed(effectName, reference);
                }
            }
        }
    }
}