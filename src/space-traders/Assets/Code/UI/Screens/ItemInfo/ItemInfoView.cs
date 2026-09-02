using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.ItemInfo
{
    public sealed class ItemInfoView : UIScreenView
    {
        [Header("Header")]
        [field: SerializeField] public Image ItemIcon { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemNameTmp { get; private set; }
        [field: SerializeField] public Button BackButton { get; private set; }
        [field: SerializeField] public Button ForwardButton { get; private set; }

        [Header("Tabs")]
        [SerializeField] private List<ItemInfoTabView> _tabs = new();

        [Header("Attributes tab")]
        [field: SerializeField] public Transform AttributesRoot { get; private set; }
        [field: SerializeField] public ItemAttributeRowView AttributeRowPrefab { get; private set; }

        public IReadOnlyList<ItemInfoTabView> Tabs => _tabs;
    }
}
