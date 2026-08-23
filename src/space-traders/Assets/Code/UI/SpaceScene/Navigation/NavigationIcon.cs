using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.SpaceScene.Navigation
{
    public sealed class NavigationIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _selectedFrame;

        private GameEntity _entity;
        private Action<GameEntity> _onSelect;
        private RectTransform _rect;

        public RectTransform Rect
        {
            get
            {
                if (_rect == null)
                    _rect = (RectTransform)transform;

                return _rect;
            }
        }

        public void Bind(GameEntity entity, Action<GameEntity> onSelect)
        {
            _entity = entity;
            _onSelect = onSelect;

            _selectButton.onClick.AddListener(OnClick);
            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            if (_selectedFrame != null)
                _selectedFrame.SetActive(selected);
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
