using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public interface IGradient 
    {
        GradientType Type { get; }
        GradientStopCollection GradientStops { get; }
    }

    public interface IBorderShader : IGradient
    {
        Borders Borders { get; set; }
    }

    public interface IRadialShader : IGradient
    {
        Vector2 Center { get; set; }
        float RadiusX { get; set; }
        float RadiusY { get; set; }
    }
}