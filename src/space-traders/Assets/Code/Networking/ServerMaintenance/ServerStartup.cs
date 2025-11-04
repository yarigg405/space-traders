using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Gameplay.Worlds;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.Identifiers;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Networking.ServerMaintenance
{
    public sealed class ServerStartup
    {
        private readonly LifetimeScope _serverScope;

        internal ServerStartup(GameLifetimeScope rootLifetimeScope)
        {
            _serverScope = rootLifetimeScope.CreateChild(builder =>
            {
                builder.Register<IdentifierService>(Lifetime.Scoped).AsImplementedInterfaces();
                builder.Register<PlayerFactory>(Lifetime.Scoped).AsSelf();

                builder.Register<PlayerBuilder>(Lifetime.Scoped).AsSelf();
                builder.Register<PlayerDataProvider>(Lifetime.Scoped).AsSelf();
                builder.Register<ClientSceneConnector>(Lifetime.Scoped).AsSelf();
                builder.Register<EcsWorldsBuilder>(Lifetime.Scoped).AsSelf();
                builder.Register<ServerWorldsController>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
                builder.Register<ServerMessengerDependencySetupper>(Lifetime.Scoped).AsImplementedInterfaces();
            });
        }

        public void StopServer()
        {
            _serverScope.Dispose();
        }
    }
}
