using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Used by <see cref="Texture"/> to provide a selector to a <see cref="UnorderedAccessView"/>.
    /// </summary>
    public sealed class UnorderedAccessViewSelector
    {
        private readonly Texture texture;

        internal UnorderedAccessViewSelector(Texture thisTexture)
        {
            texture = thisTexture;
        }

        /// <summary>
        /// Gets a specific <see cref="UnorderedAccessView" /> from this texture.
        /// </summary>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipIndex">The mip map slice index.</param>
        /// <returns>An <see cref="UnorderedAccessView" /></returns>
        public UnorderedAccessView this[int arrayOrDepthSlice, int mipIndex] { get { return texture.GetUnorderedAccessView(arrayOrDepthSlice, mipIndex); } }
    }
}
