namespace Odyssey.Renderer.Graphics.Rendering.Lights
{
    public enum LightRenderMode
    {
        Static,
        Realtime
    }

    public enum LightType
    {
        Point,
        Spotlight,
        Directional,
        Area
    }

    public enum ShadowAlgorithm
    {
        None,
        Hard,
    }
}
