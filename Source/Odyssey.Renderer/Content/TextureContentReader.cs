using System;
using Odyssey.Engine;
using Odyssey.Graphics;

namespace Odyssey.Content
{
    /// <summary>
    /// Internal class to load Texture.
    /// </summary>
    class TextureContentReader : ResourceReaderBase<Texture>
    {
        protected override Texture ReadContent(IAssetProvider readerManager, DirectXDevice device, ref ContentReaderParameters parameters)
        {
            var texture = Texture.Load(device, parameters.Stream);
            if (texture != null)
            {
                string name = string.Format("{0}_{1}", GetPrefix(texture.GetType()), parameters.AssetName);
                texture.Initialize();
                texture.Name = texture.DebugName = name;
            }
            return texture;
        }

        static string GetPrefix(Type type)
        {
            if (type == typeof(Texture1D))
                return "T1D";
            else if (type == typeof(Texture2D))
                return "T2D";
            else if (type == typeof(Texture3D))
                return "T3D";
            else if (type == typeof(TextureCube))
                return "TC";
            else return "X";
        }
    }
}
