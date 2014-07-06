using SharpDX;

namespace Odyssey.UserInterface.Style
{
    public interface IGradient
    {
        string Name { get; set; }
        GradientType Type { get; set; }
        GradientStop[] GradientStops { get; set; }
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