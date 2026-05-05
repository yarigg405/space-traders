using Assets.Code.UI.Infrastructure.Interfaces;
using UnityEngine;


namespace Assets.Code.UI.Screens.StationScreens
{
    public sealed class StationMainPresenter : IPresenter<StationMainView>
    {
        private readonly IUIManager _uiManager;

        private StationMainView _view;

        public StationMainPresenter(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        void IPresenter<StationMainView>.Show(StationMainView view, object args)
        {
            if (args is not LoadStationData data)
            {
                Debug.LogError("StationScreenData is need to open screen");
                return;
            }

            _uiManager.ClearHistory();
            _view = view;

            view.StationNameTmp.text = data.StationName;
            view.UndockButton.onClick.AddListener(ClickUndock);
        }

        void IPresenter<StationMainView>.Hide(StationMainView view)
        {
            view.UndockButton.onClick.RemoveListener(ClickUndock);
        }

        private void ClickUndock()
        {

        }
    }

    public struct LoadStationData
    {
        public int StationId;
        public string StationName;        
    }
}
