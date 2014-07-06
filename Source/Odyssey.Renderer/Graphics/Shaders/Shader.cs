using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics.Shaders
{
    public abstract class Shader : GraphicsResource
    {
        private readonly ShaderType type;
        
        public ShaderType Type { get { return type; } }

        private readonly List<TextureMapping> textures;
        private readonly IndexedCollection<long, ConstantBuffer> buffers;

        internal IndexedCollection<long, ConstantBuffer> Buffers { get { return buffers; } }

        protected Shader(DirectXDevice device, ShaderType type, string name) : base(device, name)
        {
            this.type = type;
            textures = new List<TextureMapping>();
            buffers = new IndexedCollection<long, ConstantBuffer>();
        }

        public void AddConstantBuffer(long id, ConstantBuffer buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Description.ShaderType == Type, "shaderType");
            buffers.AddItem(id, ToDispose(buffer));
        }

        public ConstantBuffer GetConstantBuffer(int index, string technique, long id = 0)
        {
            return buffers[id].First(cb => cb.Index == index && cb.Description.ShaderType == type && cb.Technique == technique);
        }

        public IEnumerable<ConstantBuffer> SelectBuffers(string technique, UpdateType type)
        {
            return from cb in buffers
                   where (cb.Description.UpdateFrequency == type && cb.Technique == technique)
                   select cb;
        }

        public IEnumerable<ConstantBuffer> SelectBuffers(string technique, long entityId, UpdateType type)
        {
            return buffers.HasItem(entityId)
                ? from cb in buffers[entityId]
                  where (cb.Description.UpdateFrequency == type && cb.Technique == technique)
                    select cb
                : Enumerable.Empty<ConstantBuffer>();
        }

        public IEnumerable<TextureMapping> SelectTextures()
        {
            return textures;
        }

        public IEnumerable<TextureMapping> SelectTextures(UpdateType type)
        {
            return from tm in textures
                   where (tm.Description.UpdateFrequency == type)
                   select tm;
        }

        [Pure]
        public bool HasConstantBuffer(int index, string technique, long id = 0)
        {
            return buffers.HasItem(id) && buffers[id].Any(cb => cb.Index == index && cb.Technique == technique);
        }

        public void AssembleBuffers()
        {
            foreach (ConstantBuffer cb in buffers.Where(cb => !cb.IsInited))
            {
                cb.Assemble(Device);
            }
        }

        public void UpdateBuffers(UpdateType updateType)
        {
            var tempBuffers = buffers.Where(cb => cb.Description.UpdateFrequency == updateType);
            foreach (ConstantBuffer cb in tempBuffers)
                cb.Update();
        }

        public bool HasTexture(int index)
        {
            return textures.Any(t => t.Description.Index == index);
        }

        public bool HasTextures()
        {
            return textures.Any();
        }

        public void AddTexture(TextureMapping tMapping)
        {
            Contract.Requires<ArgumentNullException>(tMapping != null);
            Contract.Requires<ArgumentException>(tMapping.Description.ShaderType == Type);
            textures.Add(tMapping);
        }


        public abstract void Apply(string technique, UpdateType updateType);

        public abstract void Apply(string technique, long id, UpdateType updateType);

    }
}
