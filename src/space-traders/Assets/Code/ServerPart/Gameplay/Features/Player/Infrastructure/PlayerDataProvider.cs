using Assets.Code.Infrastructure.Loading;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerDataProvider
    {
        private readonly Dictionary<ushort, string> _playerScenes = new();

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
    }
}
