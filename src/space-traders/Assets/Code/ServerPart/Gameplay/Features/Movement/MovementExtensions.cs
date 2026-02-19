using Assets.Code.Common.Extensions;
using Unity.Mathematics;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement
{
    public static class MovementExtensions
    {
        public static GameEntity StartKeepDistance(this GameEntity entity, GameEntity targetEntity, Vector2 minMaxDistance)
        {
            entity
               .ResetMovingComponents()
               .AddMovementTargetId(targetEntity.Id)
               .AddKeepDistanceMinMax(minMaxDistance)
               ;

            if (entity.CurrentSpeedModifier == 0)
                entity.ReplaceCurrentSpeedModifier(1);

            return entity;
        }

        public static GameEntity StartOrbitMoving(this GameEntity entity, float orbitRadius, GameEntity orbitingTarget)
        {
            entity
                .ResetMovingComponents()
                .AddMovementTargetId(orbitingTarget.Id)
                .AddOrbitingRadius(orbitRadius)
                ;

            if (entity.CurrentSpeedModifier == 0)
                entity.ReplaceCurrentSpeedModifier(1);

            return entity;
        }

        public static GameEntity SetWarpTo(this GameEntity entity, double2 warpCoordinates)
        {
            entity
                .ResetMovingComponents()
                .AddWarpFinishCoordinates(warpCoordinates)
                .With(x => x.isWarpPreparation = true)
                .With(x => x.isMoving = false)
                ;
            return entity;
        }

        public static GameEntity SetWarpFinished(this GameEntity entity)
        {
            entity
                .RemoveWarpDataContainer()
                .With(x => x.isWarpPreparation = false)
                .With(x => x.isWarping = false)
                .With(x => x.isMoving = true)
                ;

            return entity;
        }


        public static GameEntity ResetMovingComponents(this GameEntity entity)
        {
            if (entity.hasMovementTargetId)
                entity.RemoveMovementTargetId();

            if (entity.hasOrbitingRadius)
                entity.RemoveOrbitingRadius();

            if (entity.hasKeepDistanceMinMax)
                entity.RemoveKeepDistanceMinMax();

            return entity;
        }

        public static int[] GetMovementComponentsForReset()
        {
            int[] array =
            {
                GameComponentsLookup.CurrentSpeedModifier,
                GameComponentsLookup.TargetRotation,
                GameComponentsLookup.MovementTargetId,
                GameComponentsLookup.OrbitingRadius,
                GameComponentsLookup.KeepDistanceMinMax,
                GameComponentsLookup.WarpPreparation,
                GameComponentsLookup.WarpFinishCoordinates,
            };

            return array;
        }

        public static float GetAngleDirectionY(double2 fromPosition, double2 toPosition)
        {
            var direction = toPosition - fromPosition;
            var angleRad = math.atan2(direction.x, direction.y);
            var angleDeg = (float)(angleRad * math.TODEGREES);

            return AnglesUtil.NormalizeAngle(angleDeg);
        }
    }
}
