using System.IO;
using System.Runtime.Serialization;
using System.Windows.Navigation;
using Odyssey.Daedalus.Shaders.Techniques;
using Odyssey.Graphics.Shaders;
using SharpDX.Serialization;
using ShaderDescription = Odyssey.Daedalus.Model.ShaderDescription;

namespace Odyssey.Daedalus.Data
{
    public class ResourceManager
    {
        public Model.ShaderDescription[] Load()
        {
            ReferenceSerializer.Serialize(@"C:\Users\Adalberto\Documents\Visual Studio 2013\Projects\Odyssey\Source\Tools\Odyssey.Daedalus\Assets\Odyssey.refs");
            return new[] {
                //new ShaderDescription {Shader =  new NormalMappingVS()},
                //new ShaderDescription {Shader =  new NormalMappingPS()},
                //new ShaderDescription { Shader = new SkyboxVS()}, 
                //new ShaderDescription { Shader = new SkyboxPS()}, 
                //new ShaderDescription { Shader = new SpriteDebugPS()},
                //new ShaderDescription() { Shader = new SpriteVS()},
                //new ShaderDescription() { Shader= new SpriteDebugPS()},

                //new ShaderDescription() { Shader = new FullScreenQuadPS()},
                //new ShaderDescription() {Shader = new FullScreenQuadVS()},
                //new ShaderDescription(){Shader = new GaussianBlurPS()}, 
                //new ShaderDescription() {Shader = new GlowPS()}, 
                //new ShaderDescription(){Shader = new BloomExtractPS()}, 
                //new ShaderDescription(){Shader = new BloomCombinePS()}, 

                //new ShaderDescription() { Shader= new PhongVS()},
                //new ShaderDescription() { Shader= new PhongInstanceVS()},
                //////new ShaderDescription() { Shader= new PhongShadowsVS()},
                //new ShaderDescription() { Shader= new PhongCubeMapVS()},
                //new ShaderDescription() { Shader= new PhongPS()},
                //////new ShaderDescription() { Shader= new PhongShadowsPS()},
                //new ShaderDescription() { Shader= new PhongDiffuseMapPS()},
                //new ShaderDescription() { Shader= new PhongCubeMapPS()},

                new Model.ShaderDescription() {Shader =  new WireframeVS()},
                new Model.ShaderDescription() {Shader =  new WireframePS()},

            };
        }

        public static void Serialize(string fullPath, ShaderCollection shaderCollection)
        {
            ShaderCollection.Save(fullPath, shaderCollection);
        }

        public static void Read(string fullPath, out ShaderCollection shaderCollection)
        {

            shaderCollection = ShaderCollection.Load(fullPath);
        }

        
    }
}
