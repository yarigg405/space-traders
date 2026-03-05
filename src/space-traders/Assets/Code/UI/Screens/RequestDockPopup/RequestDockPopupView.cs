using Assets.Code.UI.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens
{
    internal sealed class RequestDockPopupView : UIScreenView
    {
        [SerializeField] private TextMeshProUGUI _stationNameTmp;
        [SerializeField] private TextMeshProUGUI _dockNumTmp;
        [field: SerializeField] public Button RequestDockBtn { get; private set; }

        internal void SetupView(uint dockId, uint stationId)
        {
            _stationNameTmp.text = $"Station: {stationId}";
            _dockNumTmp.text = $"Docking Bay: {dockId}";
        }
    }
}
