using System;

namespace Odyssey.Animations
{
    public interface IKeyFrame
    {
        /// <summary>
        /// The time of the keyframe, expressed in seconds.
        /// </summary>
        float Time { get; set; }
        object Value { get; set; }
    }
}
