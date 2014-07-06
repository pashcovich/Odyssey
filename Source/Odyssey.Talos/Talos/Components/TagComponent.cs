using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Components
{
    public class TagComponent : Component
    {
        public SortedSet<string> Tags { get; set; }

        public TagComponent() : base(ComponentTypeManager.GetType<TagComponent>())
        {
        }

    }
}
