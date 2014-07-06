using System.Collections.Generic;

namespace Odyssey.Graphics.Organization.Commands
{
    public interface IPostProcessCommand
    {
        List<Texture> Inputs { get; }
        Texture Output { get; set; }
        void SetInputs(IEnumerable<Texture> textures);
    }
}