using System.Runtime.Serialization;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;

namespace Odyssey.Content
{
    public class EffectReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            ShaderCollection shaderCollection;
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(ShaderCollection));
                shaderCollection = (ShaderCollection)dcs.ReadObject(parameters.Stream);
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
