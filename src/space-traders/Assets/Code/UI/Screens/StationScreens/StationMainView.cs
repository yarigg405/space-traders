using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.StationScreens
{
    public sealed class StationMainView : UIScreenView
    {
        [field: SerializeField] public Button UndockButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI StationNameTmp { get; private set; }
        [field: SerializeField] public TextMeshProUGUI StarSystemNameTmp { get; private set; }
    }
}
