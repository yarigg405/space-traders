using Assets.Code.Common.Time;
using Entitas;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Localization.SmartFormat.GlobalVariables;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement.Systems
{
    internal sealed class InterpolateNetworkEntitiesSystem : IExecuteSystem
    {
        private const float RenderDelayTicks = 8f;

        private readonly IGroup<GameEntity> _networkEntities;
        private readonly RemoteSnapshotBuffer _buffer;
        private readonly TickCounter _tick;
        private readonly InterpolationClock _interpolation;


        public InterpolateNetworkEntitiesSystem(GameContext game, RemoteSnapshotBuffer buffer,
            TickCounter tick, InterpolationClock interpolation)
        {
            _networkEntities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.GlobalPosition,
                GameMatcher.CurrentRotationY
                )
                .NoneOf(GameMatcher.ClientPlayer));
            _buffer = buffer;
            _tick = tick;
            _interpolation = interpolation;
        }

        void IExecuteSystem.Execute()
        {
            float renderTick = _tick.CurrentTick + _interpolation.Alpha - RenderDelayTicks;

            foreach (var entity in _networkEntities)
            {
                if (_buffer.TryGet(entity.Id, renderTick, out var pos, out var rot))
                {
                    entity.ReplaceGlobalPosition(pos);
                    entity.ReplaceCurrentRotationY(rot);
                }
            }
        }
    }
}
