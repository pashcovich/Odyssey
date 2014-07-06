using System;
using System.Runtime.InteropServices;
using Odyssey.Windows.WPF.Engine;

namespace Odyssey.Engine
{
    internal class HwndHost : System.Windows.Interop.HwndHost
    {
        private readonly HandleRef childHandle;

        public HwndHost(IntPtr childHandle)
        {
            this.childHandle = new HandleRef(this, childHandle);
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            int style = (Win32Native.GetWindowLong(childHandle, Win32Native.WindowLongType.Style)).ToInt32();
            // Removes Caption bar and the sizing border
            style = style & ~WS_CAPTION & ~WS_THICKFRAME;
            // Must be a child window to be hosted
            style |= WS_CHILD;

            Win32Native.SetWindowLong(childHandle, Win32Native.WindowLongType.Style, new IntPtr(style));

            //MoveWindow(childHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            Win32Native.SetParent(childHandle, hwndParent.Handle);

            Win32Native.ShowWindow(childHandle, false);

            return childHandle;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Win32Native.SetParent(childHandle, IntPtr.Zero);
        }

        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

        // ReSharper disable InconsistentNaming
        private const int WS_CAPTION = unchecked(0x00C00000);
        private const int WS_THICKFRAME = unchecked(0x00040000);
        private const int WS_CHILD = unchecked(0x40000000);
        // ReSharper restore InconsistentNaming
    }
}
