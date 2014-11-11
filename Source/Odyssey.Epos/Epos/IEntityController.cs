using Odyssey.Engine;

namespace Odyssey.Epos
{
    public interface IEntityController
    {
        void BindToEntity(Entity source);
        void Update(ITimeService time);
    }
}