using Assets.Code.ClientPart.Gameplay.Features;
using Assets.Code.Common.Time;
using Assets.Code.Infrastructure.States.StatesInfrastructure;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class GameLoopState : GameState, IUpdatableState
    {
        private readonly GameContext _game;
        private readonly FeaturesContainer _featuresContainer;
        private readonly ITimeService _time;
        private readonly ClockSyncService _clockSync;

        private bool _isExit;

        public GameLoopState(GameContext game,
            FeaturesContainer featuresContainer, ITimeService time, ClockSyncService clockSync)
        {
            _game = game;
            _featuresContainer = featuresContainer;
            _time = time;
            _clockSync = clockSync;
        }

        void IUpdatableState.Update()
        {
            if (_isExit) return;
            if (!_featuresContainer.IsInitialized) return;

            _featuresContainer.Tick(_time.DeltaTime * _clockSync.TimeScale);
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
