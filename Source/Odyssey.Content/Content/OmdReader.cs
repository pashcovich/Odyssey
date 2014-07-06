using Odyssey.Content.Meshes;
using Odyssey.Content.Models;
using Odyssey.Engine;
using Odyssey.Graphics.Meshes;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    [SupportedType(typeof(Model))]
    public class OmdReader : Component, IResourceReader
    {

        public object ReadContent(IAssetProvider assetManager, string resourceName, System.IO.Stream stream)
        {
            SharpDX.Serialization.BinarySerializer bs = new SharpDX.Serialization.BinarySerializer(stream, SharpDX.Serialization.SerializerMode.Read);
            Model model = bs.Load<Model>();
            return model;
        }


        
    }
}
