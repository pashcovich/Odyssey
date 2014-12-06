#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using System;
using Odyssey.UserInterface.Controls;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.UserInterface
{
    public static class TreeTraversal
    {
        public static IEnumerable<UIElement> PostOrderInteractionVisit(UIElement root)
        {
            return PostOrderVisit(root, e => e.IsEnabled && e.CanRaiseEvents);
        }

        public static IEnumerable<UIElement> PostOrderVisit(UIElement root, Func<UIElement, bool> filter)
        {
            foreach (var child in root)
            {
                if (!filter(child))
                    continue;

                foreach (var descendant in PostOrderInteractionVisit(child))
                    yield return descendant;
            }
            yield return root;
        }

        public static IEnumerable<UIElement> PreOrderVisit(UIElement root)
        {
            return PreOrderVisit(root, c => true);
        }

        public static IEnumerable<UIElement> PreOrderVisit(UIElement root, Func<UIElement, bool> filter)
        {
            if (filter(root));
                yield return root;

            foreach (var child in root)
            {
                foreach (var descendant in PreOrderVisit(child, filter))
                    yield return descendant;
            }
        }

        public static IEnumerable<UIElement> VisibleControls(UIElement root)
        {
            return PreOrderVisit(root, (c) => c.IsVisible);
        }
    }
}