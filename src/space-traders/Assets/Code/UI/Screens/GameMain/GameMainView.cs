using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.GameMain
{
    public sealed class GameMainView : UIScreenView
    {
        [field: SerializeField] public Button TradeBtn { get; private set; }
        [field: SerializeField] public Button ShipInfoBtn { get; private set; }
        [field: SerializeField] public Button WalletBtn { get; private set; }
        [field: SerializeField] public Button AllAssetBtn { get; private set; }
    }
}
