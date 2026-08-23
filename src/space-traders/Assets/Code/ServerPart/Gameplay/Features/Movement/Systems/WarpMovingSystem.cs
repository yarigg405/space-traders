using Assets.Code.Common;
using Assets.Code.Common.StaticData;
using Entitas;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class WarpMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(64);
        private readonly ConfigsStorage _configsStorage;


        public WarpMovingSystem(GameContext game, ConfigsStorage configsStorage)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.WarpDataContainer,
                GameMatcher.GlobalPosition
            ));

            _configsStorage = configsStorage;
        }


        private const float _warpAccelerationTime = 35f;
        private const float _warpMaxSpeed = 14959787070;

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var container = entity.WarpDataContainer;
                var deltaTime = GameConstants.FIXED_DELTA_TIME / 2;

                if (container.IsBraking)
                    container.CurrentWarpingTime -= deltaTime;
                else
                    container.CurrentWarpingTime += deltaTime;

                var t = container.CurrentWarpingTime / _warpAccelerationTime;
                t = Mathf.Clamp(t, 0f, 1f);
                t = Mathf.Pow(t, 4);

                if (container.IsBraking)
                {
                    var brakingMod = container.CurrentWarpingTime / container.StartBrakingTime;
                    brakingMod = Mathf.Pow(brakingMod, 4);
                    var newPos = math.lerp(container.WarpFinishPosition, container.StartBrakingPos, brakingMod);

                    entity.ReplaceGlobalPosition(newPos);
                }

                else
                {
                    container.WarpSpeedCurrent =
                        _configsStorage.WarpAccelerationCurve.Evaluate(t) * _warpMaxSpeed;

                    var deltaMove = container.WarpSpeedCurrent * deltaTime;
                    double2 newPos = CommonExtensions
                        .MoveTowards(entity.GlobalPosition, container.WarpFinishPosition, deltaMove);
                    entity.ReplaceGlobalPosition(newPos);
                }

                var remainingDistance = (container.WarpFinishPosition - entity.GlobalPosition).Magnitude();
                var distanceModifier = remainingDistance / container.WarpTotalDistance;     //1 to 0


                if (distanceModifier < 0.5 && !container.IsBraking)
                {
                    container.IsBraking = true;
                    container.StartBrakingPos = entity.GlobalPosition;
                    container.StartBrakingTime = container.CurrentWarpingTime;
                }

                if (remainingDistance < 100 || container.WarpSpeedCurrent <= 0)
                {
                    entity.SetWarpFinished();
                }
            }
        }
    }

    [Serializable]
    public struct WarpMovingTransmissionGear
    {
        public float GearTime;
        public float MaxGearSpeed;

        public WarpMovingTransmissionGear(float gearTime, float maxGearSpeed)
        {
            GearTime = gearTime;
            MaxGearSpeed = maxGearSpeed;
        }
    }
}
