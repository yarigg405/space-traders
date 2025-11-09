using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.ServerPart.Gameplay.Features.Player.Factory;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Worlds;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.ServerPart.Networking
{
    public sealed class ServerStartup
    {
        private readonly LifetimeScope _serverScope;

        internal ServerStartup(GameLifetimeScope rootLifetimeScope)
        {
            _serverScope = rootLifetimeScope.CreateChild(builder =>
            {
                builder.Register<IdentifierService>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<PlayerFactory>(Lifetime.Singleton).AsSelf();

                builder.Register<PlayerBuilder>(Lifetime.Singleton).AsSelf();
                builder.Register<PlayerDataProvider>(Lifetime.Singleton).AsSelf();
                builder.Register<ClientSceneConnector>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
                builder.Register<EcsWorldsBuilder>(Lifetime.Singleton).AsSelf();
                builder.Register<ServerWorldsController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
                builder.Register<ServerMessengerDependencySetupper>(Lifetime.Singleton).AsImplementedInterfaces();
            });
        }

        public void StopServer()
        {
            _serverScope.Dispose();
        }
    }
}
