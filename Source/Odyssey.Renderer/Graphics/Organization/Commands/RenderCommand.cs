using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;
using SharpDX;

namespace Odyssey.Graphics.Organization.Commands
{

    public abstract class RenderCommand : Command, IRenderCommand
    {
        private Effect effect;
        private readonly List<IEntity> entities;
        
        protected ModelMeshCollection Renderables { get; set; }
        protected int EntityCount { get { return entities.Count(); }}

        public IEnumerable<IEntity> Entities { get { return entities; } }

        public IEnumerable<ModelMesh> Items
        {
            get { return Renderables; }
        }

        public Effect Effect { get { return effect; } }

        protected RenderCommand(IServiceRegistry services, Effect effect, IEnumerable<IEntity> entities)
            : base(services, CommandType.Render)
        {
            Contract.Requires<ArgumentNullException>(effect != null, "effect");
            Contract.Requires<ArgumentNullException>(entities != null, "entities");
            Contract.Requires<ArgumentException>(entities.Any(), "[entities] sequence is empty");
            this.effect = ToDispose(effect);
            this.entities = new List<IEntity>(entities);
        }

        public override void Execute()
        {
            PreRender();
            Render();
            PostRender();
        }

#if DEBUG
        //public bool CheckPreconditions()
        //{
            //bool test = true;
            //    foreach (var resource in Material.Resources)
            //    {
            //        if (resource == null)
            //        {
            //            LogEvent.Engine.Warning("Null resource returned from {0}.", Material.Name);
            //            test = false;
            //            continue;
            //        }

            //        test = ((IValidable)resource).Validate();
            //    }

            //Items.SelectMany(item => item.Meshes).All(mesh => mesh.CheckFormat(Material.ItemsDescription.VertexFormat));
            //return test;
        //}
#endif

        public abstract void PreRender();
        public abstract void PostRender();
        public abstract void Render();

        public override void Unload()
        {
            RemoveAndDispose(ref effect);
            foreach (ModelMesh mesh in Renderables.Where(mesh => mesh != null))
                mesh.Dispose();
            Dispose();
        }
    }
}