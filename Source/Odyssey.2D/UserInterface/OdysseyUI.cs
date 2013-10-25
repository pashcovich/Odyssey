using Odyssey.Devices;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using System;
using System.Diagnostics.Contracts;

namespace Odyssey.UserInterface
{
    /// <summary>
    /// Odyssey User Interface manager.
    /// </summary>
    public static partial class OdysseyUI
    {
        internal static PointerEventArgs LastPointerEvent { get; private set; }

        static OdysseyUI()
        {
            LastPointerEvent = new PointerEventArgs(new PointerPoint(0, new PointerProperties(), new PointerDevice()));
        }

        /// <summary>
        /// Gets or sets a reference to current <see cref="Overlay"/> overlayed on the screen.
        /// </summary>
        /// <value>The current <see cref="Overlay"/> object.</value>
        public static Overlay CurrentOverlay
        {
            get;
            set;
        }

 


        #region UI Input

        public static void ProcessPointerMovement(PointerEventArgs e)
        {
            if (LastPointerEvent.CurrentPoint.TimeStamp < e.CurrentPoint.TimeStamp)
                LastPointerEvent = e;
            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (CurrentOverlay.CaptureControl != null)
            {
                CurrentOverlay.CaptureControl.ProcessPointerMovement(e);
                return;
            }

            //// Check whether a modal window is displayed
            //if (CurrentOverlay.WindowManager.Foremost != null && CurrentOverlay.WindowManager.Foremost.IsModal)
            //{
            //    foreach (
            //        BaseControl control in
            //            TreeTraversal.PostOrderControlInteractionVisit(CurrentOverlay.WindowManager.Foremost))
            //    {
            //        handled = control.ProcessMouseMovement(e);
            //        if (handled)
            //        {
            //            return;
            //        }
            //    }
            //    CurrentOverlay.WindowManager.Foremost.ProcessMouseMovement(e);

            //    return;
            //}

            // Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderControlInteractionVisit(CurrentOverlay))
            {
                handled = control.ProcessPointerMovement(e);
                if (handled)
                    break;
            }
            if (!handled)
                CurrentOverlay.ProcessPointerMovement(e);
        }

        public static void ProcessPointerPress(PointerEventArgs e)
        {
            if (LastPointerEvent.CurrentPoint.TimeStamp < e.CurrentPoint.TimeStamp)
                LastPointerEvent = e;
            bool handled = false;
            //// Checks whether a modal window is displayed
            //if (CurrentOverlay.WindowManager.Foremost != null && CurrentOverlay.WindowManager.Foremost.IsModal)
            //{
            //    foreach (
            //        BaseControl control in
            //            TreeTraversal.PostOrderControlInteractionVisit(CurrentOverlay.WindowManager.Foremost))
            //    {
            //        handled = control.ProcessMousePress(e);
            //        if (handled)
            //            return;
            //    }
            //    CurrentOverlay.WindowManager.Foremost.ProcessMousePress(e);
            //    return;
            //}

            // Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderControlInteractionVisit(CurrentOverlay))
            {
                handled = control.ProcessPointerPressed(e);
                if (handled)
                    break;
            }

            if (!handled)
                CurrentOverlay.ProcessPointerPressed(e);
        }

        public static void ProcessPointerRelease(PointerEventArgs e)
        {
            if (LastPointerEvent.CurrentPoint.TimeStamp < e.CurrentPoint.TimeStamp)
                LastPointerEvent = e;
            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (CurrentOverlay.CaptureControl != null)
            {
                CurrentOverlay.CaptureControl.ProcessPointerRelease(e);
                return;
            }

            //// Checks whether a modal window is displayed
            //if (CurrentOverlay.WindowManager.Foremost != null && CurrentOverlay.WindowManager.Foremost.IsModal)
            //{
            //    foreach (
            //        BaseControl control in
            //            TreeTraversal.PostOrderControlInteractionVisit(CurrentOverlay.WindowManager.Foremost))
            //    {
            //        handled = control.ProcessMouseRelease(e);
            //        if (handled)
            //            return;
            //    }
            //    CurrentOverlay.WindowManager.Foremost.ProcessMouseRelease(e);
            //    return;
            //}

            // Proceeds with the rest
            foreach (UIElement control in TreeTraversal.PostOrderControlInteractionVisit(CurrentOverlay))
            {
                handled = control.ProcessPointerRelease(e);
                if (handled)
                    break;
            }

            if (!handled)
                CurrentOverlay.ProcessPointerRelease(e);
        }

        public static void ProcessKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Key.None)
                return;

            if (CurrentOverlay.FocusedControl != null)
                CurrentOverlay.FocusedControl.ProcessKeyDown(e);
        }

        public static void ProcessKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Key.None)
                return;

            if (CurrentOverlay.FocusedControl != null)
                CurrentOverlay.FocusedControl.ProcessKeyUp(e);
        }

        //static void ProcessKeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (CurrentOverlay.FocusedControl != null)
        //        CurrentOverlay.FocusedControl.ProcessKeyPress(e);
        //}

        #endregion UI Input



    }
}