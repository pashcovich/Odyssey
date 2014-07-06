using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Graphics.Shaders
{
    public class TextureMapping
    {
        private readonly Texture texture;
        public TextureDescription Description { get; private set; }
        public Texture Texture { get { return texture; } }

        public TextureMapping(Texture texture, TextureDescription description)
        {
            this.texture = texture;
            Description = description;
        }
    }
}
