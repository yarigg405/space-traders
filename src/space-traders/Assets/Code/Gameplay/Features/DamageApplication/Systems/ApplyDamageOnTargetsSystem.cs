using Entitas;


namespace Assets.Code.Gameplay.Features.DamageApplication.Systems
{
    internal sealed class ApplyDamageOnTargetsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _damageDealers;
        private readonly GameContext _game;

        internal ApplyDamageOnTargetsSystem(GameContext game)
        {
            _damageDealers = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.TargetsBuffer,
                GameMatcher.Damage
            ));
            _game = game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var damageDealer in _damageDealers)
                foreach (var targetId in damageDealer.TargetsBuffer)
                {
                    var target = _game.GetEntityWithId(targetId);

                    if (target.hasCurrentHp)
                    {
                        target.ReplaceCurrentHp(target.CurrentHp - damageDealer.Damage);
                    }
                }
        }
    }
}