using Odyssey.Engine;

namespace Odyssey.Talos
{
    public interface IEntityController
    {
        void BindToEntity(IEntity source);
        void Update(ITimeService time);
    }
}