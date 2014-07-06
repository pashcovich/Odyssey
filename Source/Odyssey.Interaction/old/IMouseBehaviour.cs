using Odyssey.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Interaction
{
    public interface IMouseBehaviour : IInputController
    {
        void OnMouseDown(PointerEventArgs e);
        void OnMouseClick(PointerEventArgs e);
        void OnMouseUp(PointerEventArgs e);
        void OnMouseMove(PointerEventArgs e);
    }
}
