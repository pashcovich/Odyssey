using Odyssey.Engine;

namespace Odyssey.Talos.Systems
{
    public interface IUpdateableSystem: ISystem
    {
        void BeforeUpdate();
        void AfterUpdate();
        void Process(ITimeService time);
    }
}
