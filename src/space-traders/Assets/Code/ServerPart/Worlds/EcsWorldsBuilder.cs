using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features;
using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Assets.Code.ServerPart.Physics;
using Assets.Code.ServerPart.Physics.Data;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.ServerPart.Worlds
{
    public sealed class EcsWorldsBuilder
    {
        private readonly LifetimeScope _scope;
        private readonly GameWorldFiller _gameWorldFiller;

        internal EcsWorldsBuilder(LifetimeScope scope, GameWorldFiller gameWorldFiller)
        {
            _scope = scope;
            _gameWorldFiller = gameWorldFiller;
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

                builder.Register<TriggersInteractionsService>(Lifetime.Scoped).AsSelf();
                builder.Register<SystemFactory>(Lifetime.Scoped).AsImplementedInterfaces();

                builder.Register<ServerEntitiesConditionSender>(Lifetime.Scoped)
                    .AsImplementedInterfaces()
                    .WithParameter(sceneName);

                builder.Register<EntitiesSynchronizator>(Lifetime.Scoped)
                    .WithParameter(sceneName);
            });

            var container = worldScope.Container;
            var systemFactory = container.Resolve<ISystemFactory>();
            var feature = systemFactory.Create<ServerGameFeature>();

            var newWorld = new EcsWorldInstance(sceneName, feature, contexts);
            feature.Initialize();

            _gameWorldFiller.FillWorld(sceneName, contexts);
            return newWorld;
        }
    }
}
