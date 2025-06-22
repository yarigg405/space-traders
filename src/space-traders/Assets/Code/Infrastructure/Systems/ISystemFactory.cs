using Entitas;

namespace Assets.Code.Infrastructure.Systems
{
    internal interface ISystemFactory
    {
        T Create<T>() where T : ISystem;
    }
}