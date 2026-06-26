using Assets.Code.Common.DataBase;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Common.StaticData.Staff;
using Assets.Code.Infrastructure.DI;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Factory;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Gameplay.Features.Player.Services;
using Assets.Code.ServerPart.Gameplay.Features.PointsOfInterest.Factories;
using Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Factory;
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
                builder.Register<SpaceStationsFactory>(Lifetime.Singleton).AsSelf();
                builder.Register<SkyboxObjectFactory>(Lifetime.Singleton).AsSelf();

                builder.Register<PlayerBuilder>(Lifetime.Singleton).AsSelf();
                builder.Register<PlayerLocationManager>(Lifetime.Singleton).AsSelf();
                builder.Register<PlayerCharacterManager>(Lifetime.Singleton).AsSelf();
                builder.Register<PlayerSaver>(Lifetime.Singleton).AsSelf();
                builder.Register<ServerDockingService>(Lifetime.Scoped).AsSelf();

                builder.Register<ClientSceneConnector>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
                builder.Register<GameWorldFiller>(Lifetime.Singleton).AsSelf();
                builder.Register<EcsWorldsBuilder>(Lifetime.Singleton).AsSelf();
                builder.Register<ServerWorldsController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
                builder.Register<ServerInputService>(Lifetime.Singleton).AsSelf();

                builder.Register<ServerMessenger>(Lifetime.Singleton).AsSelf();
                builder.Register<ServerMessengerRouter>(Lifetime.Singleton).AsSelf();
                builder.Register<ServerMessengerDependencySetupper>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.Register<DataBaseManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<PlayersRepository>(Lifetime.Singleton).AsSelf();
                builder.Register<CharactersRepository>(Lifetime.Singleton).AsSelf();
                builder.Register<StarSystemRepository>(Lifetime.Singleton).AsSelf();
                builder.Register<PlanetsRepository>(Lifetime.Singleton).AsSelf();
                builder.Register<SpaceStationsRepository>(Lifetime.Singleton).AsSelf();
                builder.Register<CharacterLocationsRepository>(Lifetime.Singleton).AsSelf();
                builder.Register<WalletsRepository>(Lifetime.Singleton).AsSelf();

                builder.Register<CharactersCreatingService>(Lifetime.Singleton).AsSelf();
            });
        }

        public void StopServer()
        {
            _serverScope.Dispose();
        }
    }
}
