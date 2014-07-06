using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Talos.Components
{
    public partial class PostProcessComponent
    {

        public static PostProcessComponent Glow(IDirectXDeviceSettings deviceSettings)
        {
            var tDescQuarter = GetTextureDescription(deviceSettings, 0.5f);
            var tDescFull = GetTextureDescription(deviceSettings);
            var cGlow = new PostProcessComponent()
            {
                AssetName = "Glow",
                TagFilter = "Glow",
                Actions = new List<PostProcessAction>()
                {
                    new PostProcessAction(Param.Odyssey, Param.EngineActions.RenderSceneToTexture, new [] {-1}, tDescFull),
                    new PostProcessAction("PostProcess", "Default", new []{0}, tDescQuarter),
                    new PostProcessAction("PostProcess", "GaussianBlurH", new []{1}, tDescQuarter),
                    new PostProcessAction("PostProcess", "GaussianBlurV", new []{2}, tDescQuarter),
                    new PostProcessAction("PostProcess", "Default", new []{3} ,tDescFull),
                    new PostProcessAction("PostProcess", "Glow", new []{0, 4}, tDescFull, TargetOutput.BackBuffer)
                }
            };
            return cGlow;
        }
        
    }
}
