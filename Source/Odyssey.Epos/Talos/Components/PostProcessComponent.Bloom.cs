using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Graphics.PostProcessing;

namespace Odyssey.Epos.Components
{
    public partial class PostProcessComponent
    {
        public static PostProcessComponent Bloom(float downScale, TargetOutput target = TargetOutput.Backbuffer)
        {
            var cGlow = new PostProcessComponent()
            {
                AssetName = "Bloom",
                TagFilter = "Bloom",
                Target = target,
                Actions = new List<PostProcessAction>()
                {
                    new PostProcessAction(Param.Engine, Param.EngineActions.RenderSceneToTexture, new [] {-1}),
                    new PostProcessAction(Param.Shaders.PostProcess, "BloomExtract", new []{0}, scale: downScale),
                    new PostProcessAction(Param.Shaders.PostProcess, "GaussianBlurH", new []{1}, scale: downScale),
                    new PostProcessAction(Param.Shaders.PostProcess, "GaussianBlurV", new []{2}, scale: downScale),
                    new PostProcessAction(Param.Shaders.PostProcess, "BloomCombine", new []{0,3}, OutputRule.Output),
                }
            };
            return cGlow;
        }
    }
}
