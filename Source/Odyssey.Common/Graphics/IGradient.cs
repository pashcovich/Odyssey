namespace Odyssey.Graphics
{
    public interface IGradient : IColorResource
    {
        GradientStopCollection GradientStops { get; }
    }

}