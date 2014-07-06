using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    /// <summary>
    /// Current timing used for variable-step (real time) or fixed-step (application time) applications.
    /// </summary>
    internal class ApplicationTime : ITimeService
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTime" /> class.
        /// </summary>
        public ApplicationTime()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTime" /> class.
        /// </summary>
        /// <param name="totalApplicationTime">The total application time since the start of the game.</param>
        /// <param name="elapsedGameTime">The elapsed application time since the last update.</param>
        public ApplicationTime(TimeSpan totalApplicationTime, TimeSpan elapsedGameTime)
        {
            TotalApplicationTime = totalApplicationTime;
            ElapsedApplicationTime = elapsedGameTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTime" /> class.
        /// </summary>
        /// <param name="totalApplicationTime">The total application time since the start of the game.</param>
        /// <param name="elapsedGameTime">The elapsed application time since the last update.</param>
        /// <param name="isRunningSlowly">True if the game is running unexpectedly slowly.</param>
        public ApplicationTime(TimeSpan totalApplicationTime, TimeSpan elapsedGameTime, bool isRunningSlowly)
        {
            TotalApplicationTime = totalApplicationTime;
            ElapsedApplicationTime = elapsedGameTime;
            IsRunningSlowly = isRunningSlowly;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the elapsed application time since the last update
        /// </summary>
        /// <value>The elapsed application time.</value>
        public TimeSpan ElapsedApplicationTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the application is running slowly than its TargetElapsedTime. This can be used for example to render less details...etc.
        /// </summary>
        /// <value><c>true</c> if this instance is running slowly; otherwise, <c>false</c>.</value>
        public bool IsRunningSlowly { get; private set; }

        /// <summary>
        /// Gets the amount of application time since the start of the application.
        /// </summary>
        /// <value>The total application time.</value>
        public TimeSpan TotalApplicationTime { get; private set; }

        /// <summary>
        /// Gets the current frame count since the start of the game.
        /// </summary>
        public int FrameCount { get; internal set; }

        public void Update(TimeSpan totalapplicationTime, TimeSpan elapsedApplicationTime, bool isRunningSlowly)
        {
            TotalApplicationTime = totalapplicationTime;
            ElapsedApplicationTime = elapsedApplicationTime;
            IsRunningSlowly = isRunningSlowly;
        }

        #endregion
    }
}
