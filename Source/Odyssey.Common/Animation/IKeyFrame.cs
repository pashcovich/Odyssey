using System;
using Odyssey.Graphics;
using Odyssey.Serialization;

namespace Odyssey.Animation
{
    public interface IKeyFrame
    {
        TimeSpan Time { get; set; }
        object Value { get; set; }
    }
}
