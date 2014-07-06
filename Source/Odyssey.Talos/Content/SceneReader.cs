using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Talos;
using Odyssey.Talos.Components;
using Odyssey.Talos.Serialization;

namespace Odyssey.Content
{
    internal class SceneReader : ResourceReaderBase<Scene>
    {
        // <SerialzedId, ClonedId>
        private Dictionary<long, long> entityMap;

        protected override Scene ReadContent(IAssetProvider readerManager, DirectXDevice device, ref ContentReaderParameters parameters)
        {
            entityMap = new Dictionary<long, long>();
            YamlSerializer serializer = new YamlSerializer();
            SceneProxy data = serializer.Deserialize<SceneProxy>(parameters.Stream);
            Scene scene = new Scene(data.Systems, readerManager.Services);
            scene.BeginDesign();
            foreach (var kvp in data.Entities)
            {
                Entity entity = kvp.Key;
                var entityClone = scene.CreateEntity(entity.Name);
                entityMap.Add(entity.Id, entityClone.Id);
                foreach (Component component in kvp.Value)
                {
                    entityClone.RegisterComponent(component);
                }
            }
            FixParents(scene);
            scene.EndDesign();
            return scene;
        }

        void FixParents(Scene scene)
        {
            var parentComponents = scene.SelectComponents<ParentComponent>();
            foreach (var cParent in parentComponents)
            {
                long newId = entityMap[cParent.Entity.Id];
                cParent.Entity = scene.SelectEntity(newId);
            }

        }
    }
}
