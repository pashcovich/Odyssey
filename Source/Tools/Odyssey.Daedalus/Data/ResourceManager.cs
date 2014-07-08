using Odyssey.Tools.ShaderGenerator;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Shaders.Techniques;
using System.IO;
using System.Runtime.Serialization;

namespace ShaderGenerator.Data
{
    public class ResourceManager
    {
        public ShaderDescription[] Load()
        {
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
                //new ShaderDescription() {Shader = new GaussianBlurVerticalVS()},
                //new ShaderDescription() {Shader = new GaussianBlurVerticalPS()},
                //new ShaderDescription() {Shader = new GaussianBlurHorizontalVS()},
                //new ShaderDescription() {Shader = new GaussianBlurHorizontalPS()},
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

                new ShaderDescription() {Shader =  new WireframeVS()},
                new ShaderDescription() {Shader =  new WireframePS()},

            };
        }

        public static void Serialize<T>(string fullPath, T shaderData, System.Type[] derivedTypes = null)
        {
            FileStream fs = new FileStream(fullPath, FileMode.Create);
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(T), derivedTypes);
                dcs.WriteObject(fs, shaderData);
            }
            catch (SerializationException e)
            {
                Log.Daedalus.Error(e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public static T Deserialize<T>(string fullpath, System.Type[] derivedTypes = null)
        {
            FileStream fs = new FileStream(fullpath, FileMode.Open);
            T data;
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(T), derivedTypes);
                data = (T)dcs.ReadObject(fs);
            }
            catch (SerializationException e)
            {
                Log.Daedalus.Error(e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return data;
        }
    }
}
