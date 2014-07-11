namespace Odyssey.Engine
{
    internal interface IFpsTimerService
    {
        float FrameTime { get; }

        double MeasuredFPS { get; }

        void Measure();
    }
}