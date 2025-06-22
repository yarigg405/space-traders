using Entitas;
using System.Reflection;
using VContainer;


namespace Assets.Code.Infrastructure.Systems
{
    internal sealed class SystemFactory : ISystemFactory
    {
        private readonly IObjectResolver _resolver;
        private BindingFlags _flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public SystemFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T Create<T>() where T : ISystem
        {
            var constructors = typeof(T).GetConstructors(_flags);
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();

            var args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var arg = _resolver.Resolve(param.ParameterType);
                args[i] = arg;
            }

            var result = (T)constructor.Invoke(args);
            return result;
        }
    }
}
