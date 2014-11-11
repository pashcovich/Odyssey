using Odyssey.Content;
using Odyssey.Utilities.Logging;
using SharpDX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using Odyssey.Serialization;

namespace Odyssey.Graphics.Shaders
{
    [ContentReader(typeof(EffectReader))]
    public class ShaderCollection : IEnumerable<TechniqueMapping>, IDataSerializable
    {
        private const string OdysseyIdentifier = "OEFX";
        public const int Version = 0x100;

        private Dictionary<string, TechniqueMapping> techniques;
        private string name;

        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        public ShaderCollection() : this("Untitled")
        {
        }

        public ShaderCollection(string name)
        {
            this.name = name;
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

        public static ShaderCollection Load(string fullPath)
        {
            NativeFileStream fs = new NativeFileStream(fullPath,NativeFileMode.Open , NativeFileAccess.Read);
            try
            {
                ShaderCollection sc = null;
                BinarySerializer bs = new BinarySerializer(fs, SerializerMode.Read) { AllowIdentity = true };
                bs.Serialize(ref sc);
                return sc;
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
        }

        public static void Save(string fullPath, ShaderCollection shaderCollection)
        {
            NativeFileStream fs = new NativeFileStream(fullPath, NativeFileMode.Create, NativeFileAccess.Write);
            try
            {
                BinarySerializer bs = new BinarySerializer(fs, SerializerMode.Write) { AllowIdentity = true};
                bs.Serialize(ref shaderCollection);
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
        }

        public IEnumerator<TechniqueMapping> GetEnumerator()
        {
            return techniques.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.BeginChunk(OdysseyIdentifier);
            // Writes the version
            if (serializer.Mode == SerializerMode.Read)
            {
                int version = serializer.Reader.ReadInt32();
                if (version != Version)
                {
                    throw new NotSupportedException(string.Format("ModelData version [0x{0:X}] is not supported. Expecting [0x{1:X}]", version, Version));
                }
            }
            else
            {
                serializer.Writer.Write(Version);
            }

            serializer.Serialize(ref name);

            // Techniques
            serializer.BeginChunk("TECH");
            serializer.Serialize(ref techniques, serializer.Serialize, (ref TechniqueMapping t) => serializer.Serialize(ref t));
            serializer.EndChunk();

            serializer.EndChunk();
        }
    }
}