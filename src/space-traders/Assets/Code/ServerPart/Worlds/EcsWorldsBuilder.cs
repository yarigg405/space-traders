using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.ServerPart.Worlds
{
    public sealed class EcsWorldsBuilder
    {
        private readonly LifetimeScope _scope;

        internal EcsWorldsBuilder(LifetimeScope scope)
        {
            _scope = scope;
        }

        public EcsWorldInstance CreateNewServerWorld(string sceneName)
        {
            var contexts = new Contexts();

            var worldScope = _scope.CreateChild(builder =>
            {
                builder.RegisterInstance(contexts).AsSelf();
                builder.RegisterInstance(contexts.game).AsSelf();
                builder.RegisterInstance(contexts.input).AsSelf();
                builder.RegisterInstance(contexts.meta).AsSelf();

                builder.Register<SystemFactory>(Lifetime.Scoped).AsImplementedInterfaces();
                builder.Register<ServerEntitiesConditionSender>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .WithParameter(sceneName);
            });

            var container = worldScope.Container;
            var systemFactory = container.Resolve<ISystemFactory>();
            var feature = systemFactory.Create<ServerGameFeature>();

            var newWorld = new EcsWorldInstance(sceneName, feature, contexts);
            feature.Initialize();

            return newWorld;
        }
    }
}
