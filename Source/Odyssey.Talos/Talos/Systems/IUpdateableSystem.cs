using Odyssey.Engine;

namespace Odyssey.Talos.Systems
{
    public interface IUpdateableSystem: ISystem
    {
        bool BeforeUpdate();
        void AfterUpdate();
        void Process(ITimeService time);
    }
}
