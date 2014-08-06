using Odyssey.Engine;

namespace Odyssey.Animation
{
    public interface IAnimationCurve
    {
        int Length { get; }

        /// <summary>
        /// Returns the total duration of this animation, in seconds.
        /// </summary>
        float Duration { get; }

        string Name { get; }
        bool IsPlaying { get; }
        void Clear();
        void Start();
        void Stop();
        void Update(ITimeService time, object obj);
    }
}