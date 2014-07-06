using System.Diagnostics;

namespace Odyssey.Engine
{
    public class FpsTimer : IFpsTimerService
    {
        readonly Stopwatch clock;
        double totalTime;
        long frameCount;
        double measuredFPS;
        double timeElapsed;

        public double TotalTime
        {
            get { return totalTime; }
        }

        public double MeasuredFPS { get { return measuredFPS; } }

        public double FrameTimeDouble { get { return timeElapsed; } }
        public float FrameTime { get { return (float)timeElapsed; } }

        public FpsTimer()
        {
            clock = Stopwatch.StartNew();
        }

        public void Measure()
        {
            frameCount++;
            timeElapsed = (double)clock.ElapsedTicks / Stopwatch.Frequency; 
            totalTime += timeElapsed;
            if (totalTime >= 1.0f)
            {
                measuredFPS = frameCount / totalTime;
                frameCount = 0;
                totalTime = 0.0;
            }

            clock.Restart();
        }

    }
}
