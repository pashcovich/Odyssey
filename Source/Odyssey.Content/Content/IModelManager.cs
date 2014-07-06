using Odyssey.Graphics.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    public interface IModelManager
    {
        IMesh LoadModel(string filename);
    }
}
