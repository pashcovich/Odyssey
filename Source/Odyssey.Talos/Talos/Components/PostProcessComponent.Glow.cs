using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Graphics.PostProcessing;

namespace Odyssey.Talos.Components
{
    public partial class PostProcessComponent
    {
        public static PostProcessComponent Glow()
        {
            var cGlow = new PostProcessComponent()
            {
                AssetName = "Glow",
                TagFilter = "Glow",
                Actions = new List<PostProcessAction>()
                {
                    new PostProcessAction(Param.Engine, Param.EngineActions.RenderSceneToTexture, new [] {-1}),
                    new PostProcessAction("PostProcess", "Default", new []{0}, scale: 0.5f),
                    new PostProcessAction("PostProcess", "GaussianBlurH", new []{1}, scale: 0.5f),
                    new PostProcessAction("PostProcess", "GaussianBlurV", new []{2}, scale: 0.5f),
                    new PostProcessAction("PostProcess", "Default", new []{3}),
                    new PostProcessAction("PostProcess", "Glow", new []{0, 4}, OutputRule.Output)
                }
            };
            return cGlow;
        }
        
    }
}
