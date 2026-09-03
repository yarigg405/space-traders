using Assets.Code.Common.Inventory;
using Assets.Code.UI.DragAndDrop;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yrr.UI.Elements;


namespace Assets.Code.UI.Screens.StationStorage
{
    public sealed class StationItemTileView : MonoBehaviour,
        IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeableTmp _nameLabel;
        [SerializeField] private TextMeshProUGUI _amountLabel;

        private ItemSO _item;
        private int _amount;
        private int _stationId;
        private Action<ItemSO, int, Vector2> _onRightClick;
        private ItemDragController _dragController;

        public void Bind(ItemSO item, int amount, int stationId,
            Action<ItemSO, int, Vector2> onRightClick, ItemDragController dragController)
        {
            _item = item;
            _amount = amount;
            _stationId = stationId;
            _onRightClick = onRightClick;
            _dragController = dragController;
            _icon.sprite = item.Icon;
            _icon.enabled = item.Icon;
            _nameLabel.BindText(item.Id);

            if (_amountLabel)
            {
                _amountLabel.gameObject.SetActive(amount > 1);
                _amountLabel.text = $"x{amount}";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                _onRightClick?.Invoke(_item, _amount, eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _dragController?.Begin(new ItemDragPayload(_item, _amount, _stationId), _item.Icon, eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _dragController?.Move(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _dragController?.End(eventData.position);
        }
    }
}
