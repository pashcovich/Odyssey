using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Graphics.Shaders
{
    public abstract class Shader : GraphicsResource
    {
        private readonly IndexedCollection<long, ConstantBuffer> buffers;
        private readonly List<TextureMapping> textures;
        private readonly ShaderType type;

        protected Shader(DirectXDevice device, ShaderType type, string name)
            : base(device, name)
        {
            this.type = type;
            textures = new List<TextureMapping>();
            buffers = new IndexedCollection<long, ConstantBuffer>();
        }

        public ShaderType Type { get { return type; } }

        internal IndexedCollection<long, ConstantBuffer> Buffers { get { return buffers; } }

        public void AddConstantBuffer(long id, ConstantBuffer buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Description.ShaderType == Type, "shaderType");
            buffers.AddItem(id, ToDispose(buffer));
        }

        public void AddTexture(TextureMapping tMapping)
        {
            Contract.Requires<ArgumentNullException>(tMapping != null);
            Contract.Requires<ArgumentException>(tMapping.Description.ShaderType == Type);
            textures.Add(tMapping);
        }

        public abstract void Apply(string technique, UpdateType updateType);

        public abstract void Apply(string technique, long id, UpdateType updateType);

        public void AssembleBuffers()
        {
            foreach (ConstantBuffer cb in buffers.Where(cb => !cb.IsInited))
            {
                cb.Assemble();
            }
        }

        public ConstantBuffer GetConstantBuffer(int index, string technique, long id = 0)
        {
            return buffers[id].First(cb => cb.Index == index && cb.Description.ShaderType == type && cb.Technique == technique);
        }

        [Pure]
        public bool HasConstantBuffer(int index, string technique, long id = 0)
        {
            return buffers.HasItem(id) && buffers[id].Any(cb => cb.Index == index && cb.Technique == technique);
        }

        public bool HasTexture(int index)
        {
            return textures.Any(t => t.Description.Index == index);
        }

        public bool HasTextures()
        {
            return textures.Any();
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

        public void UpdateBuffers(UpdateType updateType)
        {
            var tempBuffers = buffers.Where(cb => cb.Description.UpdateFrequency == updateType);
            foreach (ConstantBuffer cb in tempBuffers)
                cb.Update();
        }
    }
}