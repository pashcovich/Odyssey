using System.Collections.Generic;

namespace Odyssey.Epos
{
    public interface IEntityProvider
    {
        IEnumerable<Entity> Entities {get;}
        Entity SelectEntity(long id);
    }
}
