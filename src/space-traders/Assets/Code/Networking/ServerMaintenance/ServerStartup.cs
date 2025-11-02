using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Gameplay.Worlds;
using Assets.Code.Infrastructure.Identifiers;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Networking.ServerMaintenance
{
    public sealed class ServerStartup
    {
        private readonly LifetimeScope _serverScope;

        public ServerStartup(LifetimeScope rootLifetimeScope)
        {
            _serverScope = rootLifetimeScope.CreateChild(builder =>
            {
                builder.Register<IdentifierService>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<PlayerFactory>(Lifetime.Scoped).AsSelf();

                builder.Register<ClientsScenesContainer>(Lifetime.Transient).AsSelf();
                builder.Register<ServerMessengerDependencySetupper>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<EcsWorldsBuilder>(Lifetime.Transient).AsSelf();
                builder.Register<ServerWorldsController>(Lifetime.Singleton).AsImplementedInterfaces();
            });
        }

        public void StopServer()
        {
            _serverScope.Dispose();   
        }
    }
}
