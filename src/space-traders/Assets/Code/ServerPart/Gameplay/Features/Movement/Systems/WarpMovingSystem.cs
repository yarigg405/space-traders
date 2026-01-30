using Assets.Code.Common;
using Assets.Code.Common.StaticData;
using Assets.Code.Common.Time;
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
        private readonly ITimeService _time;
        private readonly ConfigsStorage _configsStorage;



        public WarpMovingSystem(GameContext game, ITimeService time, ConfigsStorage configsStorage)
        {
            _time = time;

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
                var deltaTime = _time.DeltaTime;                

                if (container.IsBraking)
                    container.CurrentWarpingTime -= deltaTime;
                else
                    container.CurrentWarpingTime += deltaTime;

                var t = container.CurrentWarpingTime / _warpAccelerationTime;
                t = Mathf.Clamp(t, 0f, 1f);
                t = Mathf.Pow(t, 4);


                container.WarpSpeedPrevious = container.WarpSpeedCurrent;
                container.WarpSpeedCurrent =
                    _configsStorage.WarpAccelerationCurve.Evaluate(t) * _warpMaxSpeed;

                var deltaMove = container.WarpSpeedCurrent * deltaTime;
                double2 newPos = CommonExtensions
                    .MoveTowards(entity.GlobalPosition, container.WarpFinishPosition, deltaMove);
                entity.ReplaceGlobalPosition(newPos);

                var remainingDistance = (container.WarpFinishPosition - entity.GlobalPosition).Magnitude();
                var distanceModifier = remainingDistance / container.WarpTotalDistance;     //1 to 0                 

                container.DistanceModifier = distanceModifier;

                if (distanceModifier < 0.5 && !container.IsBraking)
                {
                    container.IsBraking = true;
                    entity.ReplaceGlobalPosition(
                       (container.WarpStartPosition + container.WarpFinishPosition) / 2);
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
