using System.Runtime.Serialization;
using Odyssey.Graphics.Shaders;
using Odyssey.Logging;
using Odyssey.Serialization;

namespace Odyssey.Content
{
    public class EffectReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            var shaderCollection = new ShaderCollection();
            try
            {
                var bs = new BinarySerializer(parameters.Stream, SerializerMode.Read) {AllowIdentity = true};
                bs.Serialize(ref shaderCollection);
            }
            catch (SerializationException e)
            {
                LogEvent.Tool.Error(e.Message);
                throw;
            }
            return shaderCollection;
        }
    }
}
