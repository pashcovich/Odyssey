using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos
{
    public interface IEntityProvider
    {
        IEnumerable<Entity> Entities {get;}
        Entity SelectEntity(long id);
    }
}
