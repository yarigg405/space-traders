using Assets.Code.Common.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;


namespace Assets.Code.Common.Serialization.Extensions
{
    public sealed class ShortNameBinder : ISerializationBinder
    {
        private readonly Dictionary<Type, string> _shortNameByType = new()
        {
            [typeof(Id)] = "id",
            [typeof(Active)] = "a",
            [typeof(ViewPath)] = "vp",
            [typeof(Destructed)] = "d",
            [typeof(SelfDestructTimer)] = "sdt",
            [typeof(GlobalPosition)] = "gp",
            [typeof(Player)] = "p",
            [typeof(PlayerNetworkId)] = "pnid",
            [typeof(MaxMoveSpeed)] = "mms",
            [typeof(MovingAcceleration)] = "ma",
            [typeof(RotationSpeed)] = "rs",
            [typeof(Velocity)] = "v",
            [typeof(VelocityAgility)] = "va",
            [typeof(CurrentSpeedModifier)] = "csm",
            [typeof(Moving)] = "m",
            [typeof(MovementTargetId)] = "mtid",
            [typeof(OrbitingRadius)] = "or",
            [typeof(KeepDistanceMinMax)] = "kd",
            [typeof(TargetRotation)] = "tr",
            [typeof(WarpPreparation)] = "wp",
            [typeof(WarpFinishCoordinates)] = "wfc",
            [typeof(Ship)] = "s",
            [typeof(Station)] = "st",
            [typeof(Planet)] = "p",
            [typeof(StationDockingBay)] = "sdb",
            [typeof(SkyboxCoordinates)] = "sc",
            [typeof(QuadrantIndex)] = "qi",
            [typeof(PhysicsRadius)] = "pr",
            [typeof(Mass)] = "ms",
            [typeof(ShipCanBeDocked)] = "scbd"
        };

        private readonly Dictionary<string, Type> _typeByShortName;

        public ShortNameBinder()
        {
            _typeByShortName = new();
            foreach (var pair in _shortNameByType)
            {
                _typeByShortName[pair.Value] = pair.Key;
            }
        }

        void ISerializationBinder.BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;

            if (_shortNameByType.TryGetValue(serializedType, out var shortName))
            {
                typeName = shortName;
                return;
            }

            throw new NotImplementedException($"No short name for type: {serializedType.FullName}");
        }

        Type ISerializationBinder.BindToType(string assemblyName, string typeName)
        {
            if (_typeByShortName.TryGetValue(typeName, out var type))
                return type;

            throw new JsonSerializationException($"No short name for type: {typeName}");
        }
    }
}
