using Odyssey.Engine;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    public static class TextureWriter
    {
        public static void Save(SharpDX.Toolkit.Graphics.IGraphicsDeviceService service, SharpDX.Direct3D11.Texture2D texture, string path)
        {
            using (var tkTexture = SharpDX.Toolkit.Graphics.Texture2D.New(service.GraphicsDevice, texture))
            {
                tkTexture.Save(path, ImageFileType.Png);
            }
        }
    }
}
