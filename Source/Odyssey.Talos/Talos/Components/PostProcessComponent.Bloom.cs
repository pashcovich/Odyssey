using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;

namespace Odyssey.Talos.Components
{
    public partial class PostProcessComponent
    {
        public static PostProcessComponent Bloom(IDirectXDeviceSettings deviceSettings, float downScale)
        {
            var tDescDownScale = GetTextureDescription(deviceSettings, downScale);
            var tDescFull = GetTextureDescription(deviceSettings);
            var cGlow = new PostProcessComponent()
            {
                AssetName = "Bloom",
                TagFilter = "Bloom",
                Actions = new List<PostProcessAction>()
                {
                    new PostProcessAction(Param.Odyssey, Param.EngineActions.RenderSceneToTexture, new [] {-1}, tDescFull),
                    new PostProcessAction("PostProcess", "BloomExtract", new []{0}, tDescDownScale),
                    new PostProcessAction("PostProcess", "GaussianBlurH", new []{1}, tDescDownScale),
                    new PostProcessAction("PostProcess", "GaussianBlurV", new []{2}, tDescDownScale),
                    new PostProcessAction("PostProcess", "BloomCombine", new []{0,3}, tDescFull, TargetOutput.BackBuffer),
                    //new PostProcessAction("PostProcess", "Default", new []{4} ,tDescFull),
                    //new PostProcessAction("PostProcess", "BloomCombine", new []{0,5} ,tDescFull, TargetOutput.BackBuffer),
                }
            };
            return cGlow;
        }
    }
}
