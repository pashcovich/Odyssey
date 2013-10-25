using Odyssey.Devices;
using Odyssey.Utils.Logging;
using System;
using Windows.UI.Core;
using KeyEventArgs = Odyssey.Devices.KeyEventArgs;
using PointerEventArgs = Odyssey.Devices.PointerEventArgs;

namespace Odyssey.UserInterface
{
    public static class WindowsRT
    {
        public static void SubscribeToKeyEvents(CoreWindow panel)
        {
            panel.KeyDown += (s, e) => OdysseyUI.ProcessKeyDown(ConvertKeyEventArgs(e));
            panel.KeyUp += (s, e) => OdysseyUI.ProcessKeyUp(ConvertKeyEventArgs(e));
        }

        public static void SubscribeToPointerEvents(CoreWindow window)
        {
            window.PointerPressed += (s, e) => OdysseyUI.ProcessPointerPress(ConvertPointerEvent(e));
            window.PointerReleased += (s, e) => OdysseyUI.ProcessPointerRelease(ConvertPointerEvent(e));
            window.PointerMoved += (s, e) => OdysseyUI.ProcessPointerMovement(ConvertPointerEvent(e));
        }

        private static PointerEventArgs ConvertPointerEvent(Windows.UI.Core.PointerEventArgs e)
        {
            var windowsPointerPoint = e.CurrentPoint;
            var windowsPointerProperties = windowsPointerPoint.Properties;
            var windowsPointerDevice = windowsPointerPoint.PointerDevice;

            PointerProperties odysseyPointerProperties = new PointerProperties()
            {
                IsLeftButtonPressed = windowsPointerProperties.IsLeftButtonPressed,
                IsRightButtonPressed = windowsPointerProperties.IsRightButtonPressed,
                MouseWheelDelta = windowsPointerProperties.MouseWheelDelta
            };

            PointerDevice odysseyPointerDevice = new PointerDevice()
            {
                PointerDeviceType = (PointerDeviceType)windowsPointerDevice.PointerDeviceType
            };

            PointerPoint odysseyPointerPoint = new PointerPoint(windowsPointerPoint.PointerId, odysseyPointerProperties, odysseyPointerDevice)
            {
                IsInContact = windowsPointerPoint.IsInContact,
                Position = windowsPointerPoint.Position.ToVector2(),
            };

            PointerEventArgs odysseyPointerEventArgs = new PointerEventArgs(odysseyPointerPoint)
            {
                Handled = e.Handled
            };

            return odysseyPointerEventArgs;
        }


        private static KeyEventArgs ConvertKeyEventArgs(Windows.UI.Core.KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case Windows.System.VirtualKey.Menu:
                    return new KeyEventArgs(Key.RightMenu);

                case Windows.System.VirtualKey.Control:
                    return new KeyEventArgs(Key.LeftControl);

                case Windows.System.VirtualKey.NumberPad0:
                    return new KeyEventArgs(Key.Number0);

                case Windows.System.VirtualKey.Decimal:
                    return new KeyEventArgs(Key.Decimal);

                default:
                    try
                    {
                        return new KeyEventArgs((Key)Enum.Parse(typeof(Key), e.VirtualKey.ToString()));
                    }
                    catch (ArgumentException)
                    {
                        LogEvent.UserInterface.Warning(string.Format("Unrecognized key: {0}", e.VirtualKey));
                        return new KeyEventArgs(Key.None);
                    }
            }
        }
    }
}