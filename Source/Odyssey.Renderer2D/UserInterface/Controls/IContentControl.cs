using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public interface IContentControl : IControl
    {
        UIElement Content { get; }
    }
}
