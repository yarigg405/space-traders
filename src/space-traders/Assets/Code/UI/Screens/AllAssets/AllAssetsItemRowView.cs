using Assets.Code.Common.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public sealed class AllAssetsItemRowView : MonoBehaviour, IPointerClickHandler
    {
        private const string LocalizationTable = "LocalizationTable";

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private TextMeshProUGUI _quantityLabel;
        [SerializeField] private TextMeshProUGUI _volumeLabel;
        [SerializeField] private Button _infoButton;

        private LocalizedString _nameString;
        private ItemSO _item;
        private int _amount;
        private Action<ItemSO> _onInfo;
        private Action<ItemSO, int, Vector2> _onRightClick;

        public void Bind(ItemSO item, int amount, Action<ItemSO> onInfo,
            Action<ItemSO, int, Vector2> onRightClick)
        {
            _item = item;
            _amount = amount;
            _onInfo = onInfo;
            _onRightClick = onRightClick;

            _icon.sprite = item.Icon;
            _icon.enabled = item.Icon;

            _quantityLabel.text = amount.ToString("N0");

            _volumeLabel.text = AttributeValueFormat.Format(ItemAttributeKeys.VolumeValueFormat, item.Volume * amount);

            _infoButton.onClick.RemoveListener(OnInfoClicked);
            _infoButton.onClick.AddListener(OnInfoClicked);

            BindName(item.Id);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                _onRightClick?.Invoke(_item, _amount, eventData.position);
        }

        private void OnInfoClicked()
        {
            _onInfo?.Invoke(_item);
        }

        private void BindName(string entryKey)
        {
            UnbindName();

            _nameString = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = entryKey
            };

            _nameString.StringChanged += OnNameChanged;
            _nameString.RefreshString();
        }

        private void UnbindName()
        {
            if (_nameString == null)
                return;

            _nameString.StringChanged -= OnNameChanged;
            _nameString = null;
        }

        private void OnNameChanged(string value)
        {
            _nameLabel.text = value;
        }

        private void OnDestroy()
        {
            UnbindName();

            _infoButton.onClick.RemoveListener(OnInfoClicked);
        }
    }
}
