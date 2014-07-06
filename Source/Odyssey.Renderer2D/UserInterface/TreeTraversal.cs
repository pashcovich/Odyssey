using Odyssey.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odyssey.UserInterface
{
    public static class TreeTraversal
    {
        public static IEnumerable<UIElement> PostOrderControlInteractionVisit(IContainer container)
        {
            foreach (UIElement ctl in container.Controls.InteractionEnabledControls)
            {
                IContainer containerControl = ctl as IContainer;
                if (containerControl != null && !containerControl.Controls.IsEmpty)
                    foreach (UIElement ctlChild in PostOrderControlInteractionVisit(containerControl))
                        yield return ctlChild;
                yield return ctl;
            }
        }

        public static IEnumerable<UIElement> PreOrderControlVisit(UIElement root)
        {
            yield return root;
            IContainer containerControl = root as IContainer;
            if (containerControl == null)
                yield break;
            foreach (UIElement ctl in containerControl.Controls)
            {
                foreach (UIElement ctlChild in PreOrderControlVisit(ctl))
                {
                    yield return ctlChild;
                }
            }

        }

        public static IEnumerable<UIElement> PreOrderHiddenControlsVisit(IContainer container)
        {
            foreach (UIElement ctl in container.Controls)
            {
                if (!ctl.IsVisible) yield return ctl;

                IContainer containerControl = ctl as IContainer;
                if (containerControl == null || containerControl.Controls.IsEmpty) continue;

                foreach (UIElement ctlChild in PreOrderHiddenControlsVisit(containerControl))
                    yield return ctlChild;
            }
        }

        public static IEnumerable<UIElement> PreOrderVisibleControlsVisit(IContainer container)
        {
            foreach (UIElement ctl in
                container.Controls.Where(ctl => ctl.IsVisible))
            {
                yield return ctl;
                IContainer containerControl = ctl as IContainer;

                if (containerControl == null || containerControl.Controls.IsEmpty) continue;

                foreach (UIElement ctlChild in
                    PreOrderVisibleControlsVisit(containerControl).Where(ctlChild => ctlChild.IsVisible))
                    yield return ctlChild;
            }
        }
    }
}