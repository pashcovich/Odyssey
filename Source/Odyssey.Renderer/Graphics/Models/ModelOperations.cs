using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Models
{
    public class ModelOperations : List<ModelOperation>
    {
        public ModelOperations()
        {
        }

        public ModelOperations(int capacity)
            : base(capacity)
        {
        }

        public ModelOperations(IEnumerable<ModelOperation> collection)
            : base(collection)
        {
        }
    }
}
