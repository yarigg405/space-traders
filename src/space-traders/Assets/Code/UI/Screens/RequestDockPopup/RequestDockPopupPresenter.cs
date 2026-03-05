using Assets.Code.Common.DataContainers;
using UnityEngine;


namespace Assets.Code.UI.Screens
{
    public sealed class RequestDockPopupPresenter
    {
        private readonly GameContext _gameContext;

        private GameEntity _dockingBayEntity;
        private GameEntity _stationEntity;

        public RequestDockPopupPresenter(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        internal void Show(RequestDockPopupView view, DockingDataContainer dockingData)
        {
            _dockingBayEntity = _gameContext.GetEntityWithId(dockingData.Dbid);
            _stationEntity = _gameContext.GetEntityWithId(dockingData.StId);

            view.SetupView(_dockingBayEntity.Id, _stationEntity.Id);
            view.RequestDockBtn.onClick.AddListener(ClickOnDock);
            view.Show();
        }

        internal void Hide(RequestDockPopupView view)
        {
            view.Hide();
            view.RequestDockBtn.onClick.RemoveListener(ClickOnDock);
        }



        private void ClickOnDock()
        {
            Debug.Log("### Dock to " + _dockingBayEntity.Id);
        }
    }
}
