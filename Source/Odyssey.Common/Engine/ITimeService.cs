using System;

namespace Odyssey.Engine
{
    public interface ITimeService
    {
        /// <summary>
        /// Gets the elapsed application time since the last update
        /// </summary>
        /// <value>The elapsed application time.</value>
        TimeSpan ElapsedApplicationTime { get; }

        /// <summary>
        /// Gets a value indicating whether the application is running slowly than its TargetElapsedTime. This can be used for example to render less details...etc.
        /// </summary>
        /// <value><c>true</c> if this instance is running slowly; otherwise, <c>false</c>.</value>
        bool IsRunningSlowly { get; }

        /// <summary>
        /// Gets the amount of application time since the start of the application.
        /// </summary>
        /// <value>The total application time.</value>
        TimeSpan TotalApplicationTime { get; }

        /// <summary>
        /// Gets the current frame count since the start of the game.
        /// </summary>
        int FrameCount { get; }

        /// <summary>
        /// Gets a value indicating the time (in seconds) this frame took to render.
        /// </summary>
        float FrameTime { get; }
    }
}