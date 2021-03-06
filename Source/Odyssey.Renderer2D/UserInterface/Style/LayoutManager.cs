﻿#region Using Directives

using System.Linq;
using Odyssey.UserInterface.Controls;
using SharpDX.Mathematics;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    public static class LayoutManager
    {
        private const float Unit = 32;
        public static float Scale = 1.0f;

        public static float Units(float units)
        {
            return Scale*Unit*units;
        }

        public static Vector3 Point(float unitsX, float unitsY, float depthZ = 0)
        {
            return Scale*Unit*new Vector3(unitsX, unitsY, depthZ);
        }

        public static void DistributeHorizontally(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            var previousPosition = Vector3.Zero;
            foreach (UIElement element in children)
            {
                element.Position += previousPosition;
                element.Arrange(new Vector3(element.DesiredSizeWithMargins.X, availableSize.Y, element.DesiredSizeWithMargins.Z));
                previousPosition = element.Position + new Vector3(element.DesiredSizeWithMargins.X, 0, 0);
            }
        }

        public static void AlignHorizontalCenter(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            float availableWidth = availableSize.X;
            foreach (UIElement element in children)
            {
                element.PositionOffsets = new Vector3((availableWidth - element.DesiredSizeWithMargins.X) / 2, element.Position.Y , element.Position.Z);
            }
        }

        public static void AlignVerticalCenter(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            float availableHeight = availableSize.Y;
            foreach (UIElement element in children)
            {
                element.PositionOffsets = new Vector3(element.Position.X, (availableHeight - element.DesiredSizeWithMargins.Y) / 2, element.Position.Z);
            }
        }

        public static void AlignVerticalBottom(UIElement parent)
        {
            float availableHeight = parent.RenderSize.Y;
            foreach (UIElement element in parent.Children)
            {
                element.Position = new Vector3(element.Position.X, availableHeight - element.Height, element.Position.Z);
            }
        }

        public static void DistributeVertically(Vector3 availableSize, IEnumerable<UIElement> children)
        {
            var previousPosition = Vector3.Zero;
            foreach (UIElement element in children)
            {
                element.Position += previousPosition;
                element.Arrange(new Vector3(availableSize.X, element.DesiredSizeWithMargins.Y, element.DesiredSizeWithMargins.Z));
                previousPosition = element.Position + new Vector3(0, element.DesiredSizeWithMargins.Y, element.Position.Z);
            }
        }
        
    }
}