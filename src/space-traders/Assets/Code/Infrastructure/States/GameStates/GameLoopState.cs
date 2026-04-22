using Assets.Code.ClientPart.Gameplay.Features;
using Assets.Code.Infrastructure.States.StatesInfrastructure;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class GameLoopState : GameState, IUpdatableState
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
            _featuresContainer.Stop();

            foreach (var entity in _game.GetEntities())
                entity.isDestructed = true;

            _featuresContainer.Cleanup();
        }
    }
}
