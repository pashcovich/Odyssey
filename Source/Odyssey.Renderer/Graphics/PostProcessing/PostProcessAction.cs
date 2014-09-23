using Odyssey.Engine;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Graphics.PostProcessing
{
    public enum TargetOutput
    {
        None,
        Backbuffer,
        Offscreen
    }
    
    public enum OutputRule
    {
        None,
        NewRenderTarget,
        FirstReusable,
        Output,
        File
    }

    public struct PostProcessAction
    {
        public string AssetName { get; private set; }
        public string Technique { get; private set; }
        public int[] InputIndices { get; private set; }
        public float Scale { get; private set; }
        public OutputRule OutputRule { get; private set; }

        public PostProcessAction(string assetName, string technique, int[] inputIndices, OutputRule outputRule = OutputRule.NewRenderTarget, float scale = 1.0f) : this()
        {
            AssetName = assetName;
            Technique = technique;
            Scale = scale;
            InputIndices = inputIndices;
            OutputRule = outputRule;
        }
    }
}
