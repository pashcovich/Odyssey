using System.Linq.Expressions;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odyssey.Talos.Initializers
{
    public class ShaderInitializer
    {
        internal static readonly Dictionary<string, Dictionary<Type, bool>> InitializerMap = new Dictionary<string, Dictionary<Type, bool>>();

        private static readonly Type[] InstanceInitializers =
        {
            typeof (MaterialInitializer), typeof (EntityInitializer),typeof(EffectInitializer)
        };

        private static readonly Type[] RegisteredInitializers =
        {
            typeof (CameraInitializer), typeof (LightInitializer), typeof (ApplicationInitializer),
            typeof (MaterialInitializer), typeof (EntityInitializer),typeof(EffectInitializer)
        };

        private readonly Technique technique;
        private readonly IServiceRegistry services;

        public ShaderInitializer(IServiceRegistry services, Technique technique)
        {
            this.services = services;
            this.technique = technique;
            Device = services.GetService<IGraphicsDeviceService>().DirectXDevice;
            FindRequiredInitializers();
        }

        public Technique Technique { get { return technique; } }

        public IServiceRegistry Services { get { return services; } }

        protected DirectXDevice Device { get; private set; }

        public void Initialize()
        {
            var initializers = InitializerMap[technique.Name].Keys.ToList();
            foreach (Type type in initializers)
            {
                IInitializer initializer = (IInitializer)Activator.CreateInstance(type, new object[]{services});
                initializer.SetupInitialization(this);
            }
        }

        public void Initialize<TInitializer, TSource>(TInitializer initializer, TSource source, InitializerParameters parameters)
            where TInitializer : Initializer<TSource>
        {
            var initializerStatus = InitializerMap[technique.Name];
            Type initializerType = typeof(TInitializer);
            bool isInstanceInitializer = InstanceInitializers.Contains(initializerType);

            if (!isInstanceInitializer && initializerStatus.ContainsKey(initializerType) && initializerStatus[initializerType])
            {
                LogEvent.Engine.Warning("Attempted to reinitialize {0}", typeof(TInitializer).Name);
                return;
            }

            initializer.Initialize(Device, source, parameters);

            if (!isInstanceInitializer && initializerStatus.ContainsKey(initializerType))
                initializerStatus[typeof(TInitializer)] = true;
        }

        private void FindRequiredInitializers()
        {
            if (InitializerMap.ContainsKey(technique.Name))
                return;

            Engine.EngineReference[] references = (from shader in technique.Mapping.Shaders
                                            from cbDesc in shader.ConstantBuffers
                                            from kvp in cbDesc.References
                                            select kvp.Value).ToArray();
            InitializerMap.Add(technique.Name, new Dictionary<Type, bool>());

            foreach (Type t in from t in RegisteredInitializers
                               let initializer = (IInitializer)Activator.CreateInstance(t, new object[] {services})
                               let result = initializer.AcceptedReferences.Intersect(references, Engine.EngineReference.Comparer)
                               where result.Any()
                               select t)
            {
                InitializerMap[technique.Name].Add(t, false);
            }
        }
    }
}