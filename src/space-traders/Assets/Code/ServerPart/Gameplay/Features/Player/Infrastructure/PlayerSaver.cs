using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerSaver
    {
        private readonly PlayerCharacterManager _characterManager;
        private readonly PlayerLocationManager _locationManager;


        public PlayerSaver(PlayerCharacterManager characterManager, PlayerLocationManager locationManager)
        {
            _characterManager = characterManager;
            _locationManager = locationManager;
        }


        internal void Save(ushort clientId, GameEntity playerEntity)
        {
            var characterId = _characterManager.GetCharacterIdForPlayer(clientId);

            if (playerEntity != null)
            {
                _locationManager.SaveGlobalPosition(characterId, playerEntity.GlobalPosition);
            }
        }
    }
}
