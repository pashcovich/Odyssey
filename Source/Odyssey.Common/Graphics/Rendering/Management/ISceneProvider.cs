using Odyssey.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering.Management
{
    public interface ISceneProvider
    {
        IEnumerable<IInteractive3D> Items { get ;}
    }
}
