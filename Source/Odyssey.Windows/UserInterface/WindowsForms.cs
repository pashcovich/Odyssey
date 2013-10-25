using Odyssey.Devices;
using Odyssey.Interaction.Multitouch;
using Odyssey.Interaction.Multitouch.WindowsForms;
using Odyssey.Utils.Logging;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyEventArgs = Odyssey.Devices.KeyEventArgs;

namespace Odyssey.UserInterface
{
    public static class WindowsForms
    {
        public static void SubscribeToKeyEvents(Form window)
        {
            window.KeyDown += (s, e) => OdysseyUI.ProcessKeyDown(ConvertKeyEvent(e));
            window.KeyUp += (s, e) => OdysseyUI.ProcessKeyUp(ConvertKeyEvent(e));
        }

        public static void SubscribeToTouchEvents(Form window)
        {
            TouchHandler touchHandler = Interaction.Multitouch.WindowsForms.Factory.CreateHandler<TouchHandler>(window);
            touchHandler.TouchDown += (s, e) => OdysseyUI.ProcessPointerPress(ConvertTouchEvent(e));
            touchHandler.TouchMove += (s, e) => OdysseyUI.ProcessPointerMovement(ConvertTouchEvent(e));
            touchHandler.TouchUp += (s, e) => OdysseyUI.ProcessPointerRelease(ConvertTouchEvent(e));
        }

        public static void SubscribeToMouseEvents(Form window)
        {
            window.MouseDown += (s, e) => OdysseyUI.ProcessPointerPress(ConvertMouseEvent(e));
            window.MouseMove += (s, e) => OdysseyUI.ProcessPointerMovement(ConvertMouseEvent(e));
            window.MouseUp += (s, e) => OdysseyUI.ProcessPointerRelease(ConvertMouseEvent(e));
        }

        private static PointerEventArgs ConvertMouseEvent(MouseEventArgs e)
        {
            PointerProperties odysseyPointerProperties = new PointerProperties()
            {
                IsLeftButtonPressed = e.Button == MouseButtons.Left,
                IsRightButtonPressed = e.Button == MouseButtons.Right,
                MouseWheelDelta = e.Delta
            };

            PointerDevice odysseyPointerDevice = new PointerDevice()
            {
                PointerDeviceType = PointerDeviceType.Mouse
            };

            PointerPoint odysseyPointerPoint = new PointerPoint((uint)0, odysseyPointerProperties, odysseyPointerDevice)
            {
                IsInContact = e.Button != MouseButtons.None,
                Position = new Vector2((float)e.Location.X, (float)e.Location.Y),
            };

            PointerEventArgs odysseyPointerEventArgs = new PointerEventArgs(odysseyPointerPoint)
            {
                Handled = false
            };

            return odysseyPointerEventArgs;
        }

        private static PointerEventArgs ConvertTouchEvent(TouchEventArgs e)
        {
            PointerProperties odysseyPointerProperties = new PointerProperties()
            {
                IsLeftButtonPressed = true,
                IsRightButtonPressed = false, // how to get right tap information?
                MouseWheelDelta = 0
            };

            PointerDevice odysseyPointerDevice = new PointerDevice()
            {
                PointerDeviceType = PointerDeviceType.Touch
            };

            PointerPoint odysseyPointerPoint = new PointerPoint((uint)0, odysseyPointerProperties, odysseyPointerDevice)
            {
                IsInContact = e.IsTouchInRange,
                Position = new Vector2((float)e.Location.X, (float)e.Location.Y),
            };

            PointerEventArgs odysseyPointerEventArgs = new PointerEventArgs(odysseyPointerPoint)
            {
                Handled = false
            };

            return odysseyPointerEventArgs;
        }

        private static KeyEventArgs ConvertKeyEvent(System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Alt:
                    return new KeyEventArgs(Key.LeftMenu);

                case Keys.Control:
                    return new KeyEventArgs(Key.LeftControl);


                default:
                    try
                    {
                        return new KeyEventArgs((Key)e.KeyData);
                    }
                    catch (ArgumentException)
                    {
                        LogEvent.UserInterface.Warning(string.Format("Unrecognized key: {0}", e.KeyCode));
                        return new KeyEventArgs(Key.None);
                    }
            }
        }
    }
}
