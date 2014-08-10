using System;
using Odyssey.Graphics;
using Odyssey.Serialization;

namespace Odyssey.Animations
{
    public interface IKeyFrame
    {
        TimeSpan Time { get; set; }
        object Value { get; set; }
    }
}
