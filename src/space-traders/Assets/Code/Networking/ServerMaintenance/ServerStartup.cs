using Assets.Code.Gameplay.Features.Player.Factory;
using Assets.Code.Gameplay.Worlds;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.Identifiers;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Networking.ServerMaintenance
{
    public sealed class ServerStartup
    {
        private readonly LifetimeScope _serverScope;

        internal ServerStartup(GameLifetimeScope rootLifetimeScope)
        {
            Debug.Log("Server Startup");
            _serverScope = rootLifetimeScope.CreateChild(builder =>
            {
                builder.Register<IdentifierService>(Lifetime.Scoped).AsImplementedInterfaces();
                builder.Register<PlayerFactory>(Lifetime.Scoped).AsSelf();

                builder.Register<ClientsScenesContainer>(Lifetime.Scoped).AsSelf();
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
