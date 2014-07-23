using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;

namespace Odyssey.Graphics.Organization
{
    [DebuggerDisplay("[{technique.Name}]: {entities.Count} items using {model.Name}")]
    public class RenderableInstruction
    {
        private readonly Technique technique;
        private readonly List<IEntity> entities;
        private readonly Model model;

        public Technique Technique
        {
            get { return technique; }
        }

        public IEnumerable<IEntity> Entities
        {
            get { return entities; }
        }

        public Model Model
        {
            get { return model; }
        }

        public RenderableInstruction(Technique technique, Model model, List<IEntity> entities )
        {
            this.technique = technique;
            this.model = model;
            this.entities = entities;
        }
    }
}
