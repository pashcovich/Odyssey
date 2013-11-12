using Odyssey.Tools.ShaderGenerator;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Shaders.Techniques;
using Odyssey.Utils.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShaderGenerator.Data
{
    public class ResourceManager
    {
        //public ShaderDescription[] Load()
        //{
        //    return new[] {
        //        //new ShaderDescription() { Shader = new SpritePS()},
        //        //new ShaderDescription() { Shader = new SpriteVS()},
        //        new ShaderDescription() { Shader= new PhongVS()},
        //        new ShaderDescription() { Shader= new PhongShadowsVS()},
        //        new ShaderDescription() { Shader= new PhongCubeMapVS()},
        //        new ShaderDescription() { Shader= new PhongPS()},
        //        new ShaderDescription() { Shader= new PhongShadowsPS()},
        //        new ShaderDescription() { Shader= new PhongDiffuseMapPS()},
        //        new ShaderDescription() { Shader= new PhongCubeMapPS()},

        //    };
        //}

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
