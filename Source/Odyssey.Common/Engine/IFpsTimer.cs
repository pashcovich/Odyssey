namespace Odyssey.Engine
{
    interface IFpsTimerService
    {
        float FrameTime { get; }
        void Measure();
        double MeasuredFPS { get; }
    }
}
