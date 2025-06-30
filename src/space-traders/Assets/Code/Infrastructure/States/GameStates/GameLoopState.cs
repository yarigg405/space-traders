using Assets.Code.Gameplay;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class GameLoopState : GameState, IUpdatableState, IPostLateTickable
    {
        private readonly GameContext _game;
        private readonly FeaturesContainer _featuresContainer;
        private bool _isExit;

        public GameLoopState(GameContext game, FeaturesContainer featuresContainer)
        {
            _game = game;
            _featuresContainer = featuresContainer;
        }

        void IUpdatableState.Update()
        {
            if (_isExit) return;
            if (!_featuresContainer.IsInitialized) return;

            _featuresContainer.Tick();
        }

        public override void Exit()
        {
            _isExit = true;
        }

        void IPostLateTickable.PostLateTick()
        {
            if (!_isExit) return;

            _featuresContainer.Stop();
            DestructEntities();
            _featuresContainer.Cleanup();
        }

        private void DestructEntities()
        {
            foreach (var entity in _game.GetEntities())
                entity.isDestructed = true;
        }
    }
}
