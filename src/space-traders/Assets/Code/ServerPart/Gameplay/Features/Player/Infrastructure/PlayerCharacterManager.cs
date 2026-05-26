using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerCharacterManager
    {
        private readonly Dictionary<ushort, int> _playerCharacters = new();


        public int GetCharacterIdForPlayer(ushort playerId)
        {
            return _playerCharacters[playerId];
        }

        public void SetCharacterForPlayer(ushort playerId, int characterId)
        {
            _playerCharacters[playerId] = characterId;
        }
    }
}
