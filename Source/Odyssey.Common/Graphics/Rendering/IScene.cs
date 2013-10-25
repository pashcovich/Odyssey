using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Rendering.Lights;
using Odyssey.Graphics.Rendering.Management;
using System;
using System.Collections.Generic;

namespace Odyssey.Graphics.Rendering
{
    public interface IScene
    {
        ISceneTree Tree { get; }
        IEnumerable<ILight> Lights { get; }
        int LightCount { get; }

        void AddLight(ILight light);
        bool ContainsLight(ILight light);
        ILight GetLight(int index);
        ILight GetLight(int index, LightType type);
        void RemoveLight(ILight light);
        void RemoveLight(int index);


        
    }
}
