#region Original License

// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Using Directives

using System;

#endregion

namespace Odyssey.Engine
{
    /// <summary>
    /// Current timing used for variable-step (real time) or fixed-step (application time) applications.
    /// </summary>
    public class ApplicationTime : ITimeService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTime" /> class.
        /// </summary>
        public ApplicationTime() {}

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

        /// <inheritdoc/>
        public float FrameTime
        {
            get { return (float) ElapsedApplicationTime.TotalSeconds; }
        }
    }
}