using Assets.Code.Common.Time;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


internal sealed class ConsumePlayerCommandSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _players;
    private readonly ServerCommandBuffer _buffer;
    private readonly TickCounter _tick;
    private readonly EntitiesSynchronizator _sync;

    public ConsumePlayerCommandSystem(GameContext game, ServerCommandBuffer buffer,
        TickCounter tick, EntitiesSynchronizator sync)
    {
        _players = game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Player,
            GameMatcher.TargetRotation
            ));

        _buffer = buffer;
        _tick = tick;
        _sync = sync;
    }

    void IExecuteSystem.Execute()
    {
        foreach (var player in _players)
        {
            if (!_buffer.TryConsume(player.Id, _tick.CurrentTick, out var input))
                continue;

            player.ReplaceMoveInput(input);

            if (input.sqrMagnitude > 0.0001f)
            {
                player.ResetMovingComponents();
                _sync.UpdateComponentsForEntity(player, 
                    MovementExtensions.GetMovementComponentsForReset());
            }
        }
    }
}