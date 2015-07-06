using SharpDX.Direct3D11;

namespace Odyssey.Graphics.Drawing
{
    public class VertexBufferOverride : IDesignerInstruction
    {
        private readonly ResourceUsage resourceUsage;

        public ResourceUsage ResourceUsage { get { return resourceUsage; }}

        public VertexBufferOverride(ResourceUsage usage)
        {
            resourceUsage = usage;
        }

        public void Execute(Designer designer)
        {
            designer.ResourceUsage = ResourceUsage;
        }
    }
}
