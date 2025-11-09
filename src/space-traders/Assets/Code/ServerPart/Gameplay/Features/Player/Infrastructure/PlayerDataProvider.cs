using Assets.Code.Infrastructure.Loading;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerDataProvider
    {
        public string GetSceneNameForPlayer(ushort playerId)
        {
            return SceneNames.GameScene1;
        }
    }
}
