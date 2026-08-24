using Assets.Code.Common;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class IntegratePlayerInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly EntitiesSynchronizator _synchronizator;

        public IntegratePlayerInputSystem(GameContext game, EntitiesSynchronizator synchronizator)
        {
            _players = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MoveInput,
                GameMatcher.TargetRotation,
                GameMatcher.CurrentSpeedModifier
                ));
            _synchronizator = synchronizator;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                var input = player.MoveInput;
                if (input.sqrMagnitude < 0.001f) continue;

                player.ReplaceTargetRotation(player.TargetRotation + input.x * 30f * GameConstants.FIXED_DELTA_TIME);

                var speed = math.clamp(player.CurrentSpeedModifier + input.y * 1f * GameConstants.FIXED_DELTA_TIME, 0f, 1f);
                player.ReplaceCurrentSpeedModifier(speed);

                _synchronizator.UpdateComponentsForEntity(player,
                    GameComponentsLookup.TargetRotation, GameComponentsLookup.CurrentSpeedModifier);
            }
        }
    }
}
