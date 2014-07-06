using SharpDX.Direct3D11;

namespace Odyssey.Graphics.PostProcessing
{
    public enum TargetOutput
    {
        NewRenderTarget,
        FirstReusable,
        BackBuffer
    }

    public struct PostProcessAction
    {
        public string AssetName { get; private set; }
        public string Technique { get; private set; }
        public Texture2DDescription TextureDescription { get; private set; }
        public int[] InputIndices { get; private set; }
        public TargetOutput Output { get; private set; }

        public PostProcessAction(string assetName, string technique,int[] inputIndices, Texture2DDescription textureDescription, TargetOutput output = TargetOutput.NewRenderTarget) : this()
        {
            AssetName = assetName;
            Technique = technique;
            TextureDescription = textureDescription;
            InputIndices = inputIndices;
            Output = output;
        }
    }
}
