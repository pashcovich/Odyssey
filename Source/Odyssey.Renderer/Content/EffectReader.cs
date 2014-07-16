using System.Runtime.Serialization;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using SharpDX.Serialization;

namespace Odyssey.Content
{
    public class EffectReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            ShaderCollection shaderCollection = new ShaderCollection();
            try
            {
                BinarySerializer bs = new BinarySerializer(parameters.Stream, SerializerMode.Read) {AllowIdentity = true};
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
