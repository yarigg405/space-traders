using Assets.Code.Common.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yrr.UI.Elements;


namespace Assets.Code.UI.Screens.ItemInfo
{
    public sealed class ItemAttributeRowView : MonoBehaviour
    {
        private const string LocalizationTable = "LocalizationTable";

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeableTmp _nameLabel;
        [SerializeField] private TextMeshProUGUI _valueLabel;

        public void Bind(ItemAttribute attribute, Sprite icon)
        {
            _icon.sprite = icon;
            _icon.enabled = icon;

            _valueLabel.text = attribute.Value;
            _nameLabel.BindText(attribute.NameKey);
        }
    }
}
