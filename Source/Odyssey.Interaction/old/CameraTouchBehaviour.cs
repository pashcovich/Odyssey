using Odyssey.Devices;
using Odyssey.Engine;
using Odyssey.Interaction;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Input;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Input
{
    public class CameraTouchBehaviour : IPointerBehaviour
    {
        private Vector2 dragStart;
        private ICamera camera;

        public CameraTouchBehaviour(ICamera camera)
        {
            this.camera = camera;
        }

        void IPointerBehaviour.OnTouchPress(object sender, PointerEventArgs e)
        {
            dragStart = e.CurrentPoint.Position;
        }

        public void OnTouchMovement(object sender, PointerEventArgs e)
        {
            Vector2 delta = (e.CurrentPoint.Position - dragStart) / 100f;
            
            camera.Move(-delta.Y);
            camera.Strafe(delta.X);
            dragStart = e.CurrentPoint.Position;
        }

        public void OnTouchRelease(object sender, PointerEventArgs e)
        {
            return;
        }

        public IInteractive3DModel Mesh
        {
            get;
            set;
        }

        void IInputController.Add()
        {
            IPointerBehaviour touchBehaviour = (IPointerBehaviour)this;
            OdysseyUI.CurrentOverlay.PointerPressed += touchBehaviour.OnTouchPress;
            OdysseyUI.CurrentOverlay.PointerMoved += touchBehaviour.OnTouchMovement;
            OdysseyUI.CurrentOverlay.PointerReleased += touchBehaviour.OnTouchRelease;
        }

        void IInputController.Remove()
        {
            IPointerBehaviour touchBehaviour = (IPointerBehaviour)this;
            OdysseyUI.CurrentOverlay.PointerPressed -= touchBehaviour.OnTouchPress;
            OdysseyUI.CurrentOverlay.PointerMoved -= touchBehaviour.OnTouchMovement;
            OdysseyUI.CurrentOverlay.PointerReleased -= touchBehaviour.OnTouchRelease;
        }

        public string Name
        {
            get { return GetType().Name; }
        }
    }
}