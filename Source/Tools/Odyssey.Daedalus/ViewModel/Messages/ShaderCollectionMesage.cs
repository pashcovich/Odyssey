#region Using Directives

using Odyssey.Graphics.Shaders;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator.ViewModel.Messages
{
    public class ShaderCollectionMesage
    {
        public ShaderCollectionMesage(ShaderCollection shaderCollection)
        {
            ShaderCollection = shaderCollection;
        }

        public ShaderCollection ShaderCollection { get; private set; }
    }
}