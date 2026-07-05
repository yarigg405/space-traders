using Assets.Code.Common.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradeItemRowView : MonoBehaviour
    {
        private const string LocalizationTable = "LocalizationTable";

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;

        private LocalizedString _labelString;

        public void Bind(ItemSO item)
        {
            if (_icon)
            {
                _icon.sprite = item.Icon;
                _icon.enabled = item.Icon;
            }

            BindLabel(item.Id);
        }

        private void BindLabel(string entryKey)
        {
            UnbindLabel();

            _labelString = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = entryKey
            };

            _labelString.StringChanged += OnLabelChanged;
            _labelString.RefreshString();
        }

        private void UnbindLabel()
        {
            if (_labelString == null)
                return;

            _labelString.StringChanged -= OnLabelChanged;
            _labelString = null;
        }

        private void OnLabelChanged(string value)
        {
            _label.text = value;
        }

        private void OnDestroy()
        {
            UnbindLabel();
        }
    }
}
