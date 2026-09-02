using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yrr.UI.Elements;


namespace Assets.Code.UI.Screens.BuySellPopups
{
    public sealed class SellItemView : UIScreenView
    {
        [field: SerializeField] public Image ItemIcon { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemNameTmp { get; private set; }

        [field: SerializeField] public TextMeshProUGUI LocationNameTmp { get; private set; }
        [field: SerializeField] public TextMeshProUGUI PriceTmp { get; private set; }

        [field: SerializeField] public TMP_InputField QuantityIF { get; private set; }
        [field: SerializeField] public Button IncreaseQuantityBtn { get; private set; }
        [field: SerializeField] public Button DecreaseQuantityBtn { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MaxQuantityTmp { get; private set; }

        [field: SerializeField] public TextMeshProUGUI TotalPriceTmp { get; private set; }
        [field: SerializeField] public LocalizeableTmp TotalVolumeTmp { get; private set; }
        [field: SerializeField] public LocalizeableTmp TotalMassTmp { get; private set; }

        [field: SerializeField] public Button ConfirmSellBtn { get; private set; }
        [field: SerializeField] public Button CancelSellBtn { get; private set; }
    }
}
