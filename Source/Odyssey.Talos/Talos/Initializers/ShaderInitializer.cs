using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Nodes;
using Odyssey.Utilities.Logging;
using SharpDX;

namespace Odyssey.Talos.Initializers
{
    public class ShaderInitializer
    {
        private static readonly Type[] RegisteredInitializers =
        {
            typeof (CameraInitializer), typeof (LightInitializer), typeof (ApplicationInitializer),
            typeof (MaterialInitializer), typeof (EntityInitializer)
        };

        private static readonly Type[] InstanceInitializers =
        {
            typeof (MaterialInitializer), typeof (EntityInitializer)
        };

        private static readonly Dictionary<string, Dictionary<Type, bool>> InitializerMap = new Dictionary<string, Dictionary<Type, bool>>();
        
        private readonly IServiceRegistry services;
        private readonly Effect effect;
        private readonly TechniqueMapping technique;
        protected DirectXDevice Device { get; private set; }

        public IServiceRegistry Services { get { return services; } }
        public Effect Effect { get { return effect; } }
        public TechniqueMapping Technique { get { return technique; } }

        public ShaderInitializer(IServiceRegistry services, Effect effect, TechniqueMapping technique)
        {
            this.services = services;
            this.technique = technique;
            this.effect = effect;
            Device = services.GetService<IOdysseyDeviceService>().DirectXDevice;
            FindRequiredInitializers();
        }

        private void FindRequiredInitializers()
        {
            if (InitializerMap.ContainsKey(effect.Name))
                return;

            EngineReference[] references = (from shader in technique.Shaders
                from cbDesc in shader.ConstantBuffers
                from kvp in cbDesc.References
                select kvp.Value).ToArray();
            InitializerMap.Add(effect.Name, new Dictionary<Type, bool>());

            foreach (Type t in from t in RegisteredInitializers
                let initializer = (IInitializer) Activator.CreateInstance(t)
                let result = initializer.AcceptedReferences.Intersect(references)
                where result.Any()
                select t)
            {
                InitializerMap[effect.Name].Add(t, false);
            }
        }

        public void Initialize()
        {
            var initializers = InitializerMap[effect.Name].Keys.ToList();
            foreach (Type type in initializers)
            {
                IInitializer initializer = (IInitializer)Activator.CreateInstance(type);
                initializer.SetupInitialization(this);
            }
        }

        //public void InitializeMaterial(IEntity entity)
        //{
        //    InitializerParameters parameters = new InitializerParameters(entity.Id, services, Initializer.InstanceSelector);
        //    Initialize<MaterialInitializer, IEntity>(entity, parameters);
        //}

        //public void InitializeEntity(IEntity entity)
        //{
        //    InitializerParameters parameters = new InitializerParameters(entity.Id, services, Initializer.InstanceSelector);
        //    Initialize<EntityInitializer, IEntity>(entity, parameters);
        //}



        //public void InitializeCamera(CameraNode cameraNode)
        //{
        //    InitializerParameters parameters = new InitializerParameters(-1, services, Initializer.StaticSelector);
        //    Initialize<CameraInitializer, CameraNode>(cameraNode, parameters);
        //}

        //public void InitializeApplication()
        //{
        //    InitializerParameters parameters = new InitializerParameters(-1, services, Initializer.StaticSelector);
        //    Initialize<ApplicationInitializer, IDirectXDeviceSettings>(services.GetService<IDirectXDeviceSettings>(), parameters);
        //}


        public void Initialize<TInitializer, TSource>(TInitializer initializer, TSource source, InitializerParameters parameters)
            where TInitializer : Initializer<TSource>
        {
            var initializerStatus = InitializerMap[effect.Name];
            Type initializerType = typeof (TInitializer);
            bool isInstanceInitializer = InstanceInitializers.Contains(initializerType);

            if (!isInstanceInitializer && initializerStatus.ContainsKey(initializerType) && initializerStatus[initializerType])
            {
                LogEvent.Engine.Warning("Attempted to reinitialize {0}", typeof (TInitializer).Name);
                return;
            }

            initializer.Initialize(Device, effect, source, parameters);

            if (!isInstanceInitializer && initializerStatus.ContainsKey(initializerType))
                initializerStatus[typeof (TInitializer)] = true;
        }
    }
}
