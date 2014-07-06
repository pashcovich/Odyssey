using Odyssey.Content.Shaders;
using Odyssey.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit.Graphics;

namespace Odyssey.Content
{
    [SupportedType(typeof(ShaderCollection))]
    public class EffectReader : IResourceReader
    {
        public object ReadContent(IAssetProvider assetManager, string resourceName, System.IO.Stream stream)
        {
            ShaderCollection shaderCollection = null;
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(ShaderCollection));
                shaderCollection = (ShaderCollection)dcs.ReadObject(stream);
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
