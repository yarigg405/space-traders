using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Assets.Code.UI;
using UnityEngine;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class SpaceStationState : GameState
    {
        private readonly IScenesLoader _scenesLoader;
        private readonly IUIManager _uiManager;

        public SpaceStationState(IScenesLoader scenesLoader, IUIManager uiManager)
        {
            _scenesLoader = scenesLoader;
            _uiManager = uiManager;
        }

        public override void Enter()
        {
            _scenesLoader.LoadScene(SceneNames.StationScene, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            Debug.LogError("### Setup UI here");
            _uiManager.ClearHistory();
        }
    }
}
