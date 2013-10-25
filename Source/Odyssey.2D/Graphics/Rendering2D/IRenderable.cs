using Odyssey.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface
{
    public interface IRenderable
    {
        void Initialize(IDirectXProvider directX);
        void Render(IDirectXTarget target);
    }
}
