using Odyssey.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Interaction
{
    public interface IPointerBehaviour : IObjectController
    {
        void OnTouchPress(object sender, PointerEventArgs e);
        void OnTouchMovement(object sender, PointerEventArgs e);
        void OnTouchRelease(object sender, PointerEventArgs e);
    }
}
