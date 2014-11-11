using Odyssey.Engine;

namespace Odyssey.Epos.Systems
{
    public interface IUpdateableSystem: ISystem
    {
        bool BeforeUpdate();
        void AfterUpdate();
        void Process(ITimeService time);
    }
}
