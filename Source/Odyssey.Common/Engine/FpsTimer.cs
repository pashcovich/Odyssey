using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    public class FpsTimer
    {
        Stopwatch clock;
        double totalTime;
        long frameCount;
        double measuredFPS;
        double timeElapsed;

        public double TotalTime
        {
            get { return totalTime; }
        }

        public double MeasuredFPS { get { return measuredFPS; } }

        public double FrameTime { get { return timeElapsed; } }
        public float FrameTimeFloat { get { return (float)timeElapsed; } }

        public FpsTimer()
        {
            clock = Stopwatch.StartNew();
        }

        public void Measure()
        {
            frameCount++;
            timeElapsed = (double)clock.ElapsedTicks / Stopwatch.Frequency; ;
            totalTime += timeElapsed;
            if (totalTime >= 1.0f)
            {
                measuredFPS = (double)frameCount / totalTime;
                frameCount = 0;
                totalTime = 0.0;
            }

            clock.Restart();
        }

    }
}
