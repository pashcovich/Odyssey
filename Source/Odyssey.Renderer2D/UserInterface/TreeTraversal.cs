using Odyssey.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odyssey.UserInterface
{
    public static class TreeTraversal
    {
        public static IEnumerable<UIElement> PostOrderInteractionVisit(UIElement root)
        {
            IContainer containerControl = root as IContainer;
            if (containerControl != null)
                foreach (UIElement control in containerControl.Controls.InteractionEnabledControls)
                    foreach (UIElement ctlChild in PostOrderInteractionVisit(control))
                        yield return ctlChild;
            else
            {
                var contentControl = root as IContentControl;
                if (contentControl != null)
                    yield return contentControl.Content;
            }
            yield return root;
        }

        public static IEnumerable<UIElement> PreOrderVisit(UIElement root)
        {
            yield return root;
            IContainer containerControl = root as IContainer;
            if (containerControl != null)
                foreach (UIElement control in containerControl.Controls)
                    foreach (UIElement ctlChild in PreOrderVisit(control))
                        yield return ctlChild;
            else
            {
                var contentControl = root as IContentControl;
                if (contentControl != null)
                    yield return contentControl.Content;
            }

        }

        internal static IEnumerable<UIElement> PreOrderContainerVisit(UIElement root)
        {
            yield return root;
            IContainer containerControl = root as IContainer;
            if (containerControl != null)
                foreach (UIElement control in containerControl.Controls)
                    foreach (UIElement ctlChild in PreOrderVisit(control))
                        yield return ctlChild;
        }


    }
}