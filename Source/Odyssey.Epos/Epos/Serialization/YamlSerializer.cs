using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Epos.Components;
using Odyssey.Epos.Systems;
using SharpDX;
using SharpYaml;
using SharpYaml.Serialization;

namespace Odyssey.Epos.Serialization
{
    public class YamlSerializer
    {
        private readonly Serializer serializer;

        public YamlSerializer()
        {
            SerializerSettings settings = new SerializerSettings
            {
                EmitShortTypeName = true,
                EmitDefaultValues = false,
                LimitPrimitiveFlowSequence = 1
            };
            settings.RegisterAssembly(this.GetType().GetTypeInfo().Assembly);
            
            //settings.RegisterTagMapping("Entity", typeof(YamlEntity));
            //settings.RegisterTagMapping("ShaderLoadingSystem", typeof(ContentLoadingSystem<ShaderComponent>));
            //settings.RegisterTagMapping("ModelLoadingSystem", typeof(ContentLoadingSystem<ModelComponent>));
            //settings.RegisterTagMapping("TextureLoadingSystem", typeof(ContentLoadingSystem<ContentComponent>));
            //settings.RegisterTagMapping("TextureCubeLoadingSystem", typeof(ContentLoadingSystem<TextureCubeComponent>));
            serializer = new Serializer(settings);
        }

        public string Serialize(object data)
        {
            return serializer.Serialize(data);
        }

        public T Deserialize<T>(Stream stream)
        {
            return serializer.Deserialize<T>(stream);
        }

    }
}
