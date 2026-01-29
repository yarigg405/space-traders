using Assets.Code.Common;
using Assets.Code.Common.StaticData;
using Assets.Code.Common.Time;
using Entitas;
using System;
using System.Collections.Generic;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class WarpMovingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(64);
        private readonly ITimeService _time;
        private readonly ConfigsStorage _configsStorage;

        private readonly WarpMovingTransmissionGear[] _transmissionSetup = new WarpMovingTransmissionGear[]
        {
            new WarpMovingTransmissionGear(0, 0),
            new WarpMovingTransmissionGear(7, 5),
            new WarpMovingTransmissionGear(7, 1000),
            new WarpMovingTransmissionGear(6, 50000),
            new WarpMovingTransmissionGear(5, 500000),
            new WarpMovingTransmissionGear(15, 1000000),
            new WarpMovingTransmissionGear(15, 100000000),
            new WarpMovingTransmissionGear(999999, 1000000000),
        };

        public WarpMovingSystem(GameContext game, ITimeService time, ConfigsStorage configsStorage)
        {
            _time = time;

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.WarpDataContainer,
                GameMatcher.GlobalPosition
            ));
            _configsStorage = configsStorage;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                var container = entity.WarpDataContainer;
                var remainingDistance = (container.WarpFinishPosition - entity.GlobalPosition).Magnitude();
                //entity.ReplaceGlobalPosition(container.WarpFinishPosition);
                //entity.SetWarpFinished();



                var currentGear = _transmissionSetup[container.WarpGear];
                var previous = _transmissionSetup[container.WarpGear - 1];


                if (container.IsBraking)
                    container.CurrentWarpingTime -= _time.DeltaTime;
                else
                    container.CurrentWarpingTime += _time.DeltaTime;


                if (container.IsBraking)
                {
                    var targetSpeed = entity.MaxMoveSpeed + currentGear.MaxSpeedModifier;
                    var speedT = container.CurrentWarpingTime / currentGear.GearTime;
                    var evaluated = _configsStorage.WarpAccelerationCurve.Evaluate(speedT);
                    if (evaluated < 0)
                    {
                        container.WarpGear--;
                        var ccurrentGear = _transmissionSetup[container.WarpGear];
                        container.CurrentWarpingTime = ccurrentGear.GearTime;
                        continue;
                    }

                    container.WarpSpeedCurrent = previous.MaxSpeedModifier + entity.MaxMoveSpeed + evaluated * targetSpeed;
                    container.BrakingStartSpeed = container.WarpSpeedCurrent;
                    double deltaMove = container.WarpSpeedCurrent * _time.DeltaTime;
                    double2 newPos = CommonExtensions
                        .MoveTowards(entity.GlobalPosition, container.WarpFinishPosition, deltaMove);
                    entity.ReplaceGlobalPosition(newPos);
                }

                else
                {
                    var targetSpeed = entity.MaxMoveSpeed + currentGear.MaxSpeedModifier;
                    var speedT = container.CurrentWarpingTime / currentGear.GearTime;
                    var evaluated = _configsStorage.WarpAccelerationCurve.Evaluate(speedT);
                    container.WarpSpeedCurrent = previous.MaxSpeedModifier * entity.MaxMoveSpeed + evaluated * targetSpeed;
                    container.BrakingStartSpeed = container.WarpSpeedCurrent;
                    double deltaMove = container.WarpSpeedCurrent * _time.DeltaTime;
                    double2 newPos = CommonExtensions
                        .MoveTowards(entity.GlobalPosition, container.WarpFinishPosition, deltaMove);
                    entity.ReplaceGlobalPosition(newPos);

                    if (evaluated >= 1)
                    {
                        container.WarpGear++;
                        container.CurrentWarpingTime = 0;
                    }
                }

                if (remainingDistance < 100)
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
        public int MaxSpeedModifier;

        public WarpMovingTransmissionGear(float gearTime, int maxSpeedModifier)
        {
            GearTime = gearTime;
            MaxSpeedModifier = maxSpeedModifier;
        }
    }
}
