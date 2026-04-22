using Assets.Code.Infrastructure.Loading;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerDataProvider
    {
        private readonly Dictionary<ushort, string> _playerScenes = new();
        private readonly Dictionary<ushort, int> _playerCharacters = new();

        public string GetSceneNameForPlayer(ushort playerId)
        {
            if (_playerScenes.ContainsKey(playerId))
                return _playerScenes[playerId];

            return SceneNames.GameScene1;
        }

        public void SetPlayerScene(ushort playerId, string sceneName)
        {
            _playerScenes[playerId] = sceneName;
        }

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
