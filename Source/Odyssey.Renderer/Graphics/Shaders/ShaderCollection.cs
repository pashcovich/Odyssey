using Odyssey.Content;
using Odyssey.Utilities.Logging;
using SharpDX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;

namespace Odyssey.Graphics.Shaders
{
    [DataContract]
    [ContentReader(typeof(EffectReader))]
    public class ShaderCollection : IEnumerable<TechniqueMapping>
    {
        [DataMember]
        private readonly Dictionary<string, TechniqueMapping> techniques;

        [DataMember]
        public string Name { get; internal set; }

        public ShaderCollection(string name)
        {
            Name = name;
            techniques = new Dictionary<string, TechniqueMapping>();
        }

        public void Add(TechniqueMapping technique)
        {
            techniques.Add(technique.Name, technique);
        }

        public void Add(string techniqueName, ShaderDescription shaderObject)
        {
            TechniqueMapping tMapping = techniques[techniqueName];
            tMapping.Set(shaderObject);
        }

        [Pure]
        public bool Contains(string techniqueName)
        {
            return techniques.ContainsKey(techniqueName);
        }

        [Pure]
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

        public TechniqueMapping this[string key]
        {
            get { return techniques[key]; }
        }

        public static ShaderCollection Load(string name)
        {
            return Load(Global.EffectPath, name);
        }

        public static ShaderCollection Load(string path, string name)
        {
            NativeFileStream fs = new NativeFileStream(string.Format("{0}\\{1}.ofx", path, name),
                NativeFileMode.Open, NativeFileAccess.Read);
            ShaderCollection shaderCollection;
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(ShaderDescription));
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}