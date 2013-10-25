using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Utils.Logging;
using SharpDX.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;

namespace Odyssey.Content.Shaders
{
    [DataContract]
    public class ShaderCollection:IEnumerable<TechniqueMapping>
    {
        [DataMember]
        Dictionary<string, TechniqueMapping> techniques;

        [DataMember]
        public string Name { get; internal set; }

        public ShaderCollection(string name)
        {
            Name = name;
            techniques = new Dictionary<string, TechniqueMapping>();
        }

        public void Add(TechniqueMapping technique)
        {
            Contract.Requires<ArgumentException>(!Contains(technique.Name));
            techniques.Add(technique.Name, technique);
        }

        public void Add(string techniqueName, ShaderObject shaderObject)
        {
            Contract.Requires<ArgumentException>(Contains(techniqueName));
            TechniqueMapping tMapping = techniques[techniqueName];
            tMapping.Set(shaderObject);
        }

        public bool Contains(string techniqueName)
        {
            return techniques.ContainsKey(techniqueName);
        }

        public bool Contains(TechniqueKey key)
        {
            return techniques.Values.Any(t => t.Key == key);
        }

        public TechniqueMapping Get(TechniqueKey key)
        {
            Contract.Requires<ArgumentException>(Contains(key));
            return techniques.Values.First(tm => tm.Key == key);
        }

        public TechniqueMapping Get(string techniqueName)
        {
            Contract.Requires<ArgumentException>(Contains(techniqueName));
            return techniques.Values.First(t => t.Name == techniqueName);
        }

        public static ShaderCollection Load (string name)
        {
            return Load(Global.EffectPath, name);
        }

        public static ShaderCollection Load(string path, string name)
        {
            NativeFileStream fs = new NativeFileStream(string.Format("{0}\\{1}.ofx", path, name),
                NativeFileMode.Open, NativeFileAccess.Read);
            ShaderCollection shaderCollection = null;
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(ShaderObject));
                shaderCollection = (ShaderCollection)dcs.ReadObject(fs);
            }
            catch (SerializationException e)
            {
                LogEvent.Tool.Error(e.Message);
                throw;
            }
            finally
            {
                fs.Dispose();
            }

            return shaderCollection;
        }

        public IEnumerator<TechniqueMapping> GetEnumerator()
        {
            return techniques.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
