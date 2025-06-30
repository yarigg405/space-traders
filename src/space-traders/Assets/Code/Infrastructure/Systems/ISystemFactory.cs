using Entitas;

namespace Assets.Code.Infrastructure.Systems
{
    public interface ISystemFactory
    {
        T Create<T>() where T : ISystem;
    }
}