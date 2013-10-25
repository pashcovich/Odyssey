using Odyssey.Engine;
using Odyssey.UserInterface;
using SharpDX;
using SharpDX.Direct2D1;
using System.Collections.Generic;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public interface IControlD2D
    {
        Brush BorderBrush { get; }
        Brush Foreground { get; }
    }

    public interface IShapeD2D : IShape
    {
        Brush FillBrush { get; }
        Brush StrokeBrush { get; }
    }
}