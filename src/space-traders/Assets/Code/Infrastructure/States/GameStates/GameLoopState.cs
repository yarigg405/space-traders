using Assets.Code.Gameplay;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Assets.Code.Infrastructure.Systems;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class GameLoopState : GameState, IUpdatableState, IPostLateTickable
    {
        private readonly GameContext _game;
        private readonly ISystemFactory _systems;

        private GameFeature _gameFeature;
        private bool _isExit;

        public GameLoopState(GameContext game, ISystemFactory systems)
        {
            _game = game;
            _systems = systems;
        }

        public override void Enter()
        {
            _gameFeature = _systems.Create<GameFeature>();
            _gameFeature.Initialize();
        }

        void IUpdatableState.Update()
        {
            if (_isExit) return;

            _gameFeature.Execute();
            _gameFeature.Cleanup();
        }

        public override void Exit()
        {
            _isExit = true;
        }

        void IPostLateTickable.PostLateTick()
        {
            if (!_isExit) return;

            _gameFeature.DeactivateReactiveSystems();
            _gameFeature.ClearReactiveSystems();

            DestructEntities();

            _gameFeature.Cleanup();
            _gameFeature.TearDown();
        }

        private void DestructEntities()
        {
            foreach (var entity in _game.GetEntities())
                entity.isDestructed = true;
        }
    }
}
