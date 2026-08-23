using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.SpaceScene.Overview
{
    public sealed class OverviewRow : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameTmp;
        [SerializeField] private TextMeshProUGUI _distanceTmp;
        [SerializeField] private TextMeshProUGUI _typeTmp;
        [SerializeField] private Button _selectButton;

        private GameEntity _entity;
        private Action<GameEntity> _onSelect;

        public GameEntity Entity => _entity;

        public void Bind(GameEntity entity, Action<GameEntity> onSelect)
        {
            _entity = entity;
            _onSelect = onSelect;
            _nameTmp.text = entity.GetName();
            _typeTmp.text = entity.GetSpaceObjectType();
            _selectButton.onClick.AddListener(OnClick);
        }

        public void SetDistance(string text)
        {
            _distanceTmp.text = text;
        }

        private void OnClick()
        {
            _onSelect?.Invoke(_entity);
        }

        private void OnDestroy()
        {
            _selectButton.onClick.RemoveListener(OnClick);
        }
    }
}
