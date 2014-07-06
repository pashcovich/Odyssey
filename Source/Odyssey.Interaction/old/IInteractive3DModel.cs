using Odyssey.Devices;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Interaction
{
    public interface IInteractive3DModel : IInteractive3D
    {
        event EventHandler<PointerEventArgs> PointerPressed;

        event EventHandler<PointerEventArgs> PointerReleased;

        event EventHandler<PointerEventArgs> Tapped;

        event EventHandler<PointerEventArgs> PointerMoved;

        event EventHandler<KeyEventArgs> KeyDown;

        event EventHandler<KeyEventArgs> KeyPress;

        event EventHandler<KeyEventArgs> KeyUp;

        void ProcessMouseEvent(MouseEventType type, PointerEventArgs e);

        void SetController<T>(T inputController) where T : class, IObjectController, IInputController;

        bool RemoveController<T>(T inputController) where T : class,IObjectController, IInputController;

        bool HasController(string controllerName);

        T GetController<T>() where T : class,IObjectController, IInputController;

    }


}
