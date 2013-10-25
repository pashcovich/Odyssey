using Odyssey.UserInterface.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Odyssey.UserInterface
{
    public static class TreeTraversal
    {
        public static IEnumerable<UIElement> PostOrderControlInteractionVisit(IContainer container)
        {
            foreach (UIElement ctl in container.PrivateControlCollection.InteractionEnabledControls)
            {
                IContainer containerControl = ctl as IContainer;
                if (containerControl != null && !containerControl.PrivateControlCollection.IsEmpty)
                    foreach (UIElement ctlChild in PostOrderControlInteractionVisit(containerControl))
                        yield return ctlChild;
                yield return ctl;
            }
        }

        public static IEnumerable<UIElement> PreOrderControlVisit(IContainer container)
        {
            foreach (UIElement ctl in container.PrivateControlCollection.AllControls)
            {
                yield return ctl;
                IContainer containerControl = ctl as IContainer;
                if (containerControl == null || containerControl.PrivateControlCollection.IsEmpty)
                    continue;

                foreach (UIElement ctlChild in PreOrderControlVisit(containerControl))
                    yield return ctlChild;
            }
        }

        public static IEnumerable<UIElement> PreOrderHiddenControlsVisit(IContainer container)
        {
            foreach (UIElement ctl in container.PrivateControlCollection)
            {
                if (!ctl.IsVisible) yield return ctl;

                IContainer containerControl = ctl as IContainer;
                if (containerControl == null || containerControl.PrivateControlCollection.IsEmpty) continue;

                foreach (UIElement ctlChild in PreOrderHiddenControlsVisit(containerControl))
                    yield return ctlChild;
            }
        }

        public static IEnumerable<UIElement> PreOrderVisibleControlsVisit(IContainer container)
        {
            foreach (UIElement ctl in
                container.PrivateControlCollection.Where(ctl => ctl.IsVisible))
            {
                yield return ctl;
                IContainer containerControl = ctl as IContainer;

                if (containerControl == null || containerControl.PrivateControlCollection.IsEmpty) continue;

                foreach (UIElement ctlChild in
                    PreOrderVisibleControlsVisit(containerControl).Where(ctlChild => ctlChild.IsVisible))
                    yield return ctlChild;
            }
        }
    }
}