using System.Collections.Generic;
using Odyssey.Graphics;

namespace Odyssey.Organization.Commands
{
    public interface IPostProcessCommand
    {
        IEnumerable<Texture> Inputs { get; }
        Texture Output { get; }
        void SetInputs(IEnumerable<Texture> textures);
    }
}