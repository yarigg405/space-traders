using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CleanupTriggerEventHandlersSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _collisionBuffers;
        private readonly IGroup<GameEntity> _triggerEnterHandlers;
        private readonly IGroup<GameEntity> _triggerExitHandlers;
        private readonly IGroup<GameEntity> _triggerStayHandlers;

        public CleanupTriggerEventHandlersSystem(GameContext game)
        {
            _collisionBuffers = game.GetGroup(GameMatcher.CollisionsBuffer);
            _triggerEnterHandlers = game.GetGroup(GameMatcher.TriggerEnterEventHandler);
            _triggerExitHandlers = game.GetGroup(GameMatcher.TriggerExitEventHandler);
            _triggerStayHandlers = game.GetGroup(GameMatcher.TriggerStayEventHandler);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _collisionBuffers)
                entity.CollisionsBuffer.Clear();

            foreach (var entity in _triggerEnterHandlers)
                entity.TriggerEnterEventHandler.Clear();

            foreach (var entity in _triggerExitHandlers)
                entity.TriggerExitEventHandler.Clear();

            foreach (var entity in _triggerStayHandlers)
                entity.TriggerStayEventHandler.Clear();
        }
    }
}
