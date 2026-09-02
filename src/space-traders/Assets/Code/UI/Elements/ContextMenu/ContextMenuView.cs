using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yrr.Utils;


namespace Assets.Code.UI.Elements
{
    public sealed class ContextMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Transform _buttonsRoot;
        [SerializeField] private ContextMenuButtonView _buttonPrefab;
        [SerializeField] private Button _backdrop;

        private void Awake()
        {
            _backdrop.onClick.AddListener(Close);

            Close();
        }

        public void Open(Vector2 screenPosition, IReadOnlyList<ContextMenuEntry> entries)
        {
            _buttonsRoot.ClearChildren();

            foreach (var entry in entries)
            {
                var button = Instantiate(_buttonPrefab, _buttonsRoot);
                button.Bind(entry, Close);
            }

            _root.SetActive(true);
            PlaceAt(screenPosition);
        }

        private void PlaceAt(Vector2 screenPosition)
        {
            if (_panel.parent is RectTransform parent &&
                RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, screenPosition, null, out var world))
            {
                _panel.position = world;
            }
            else
            {
                _panel.position = screenPosition;
            }
        }

        public void Close()
        {
            _buttonsRoot.ClearChildren();
            _root.SetActive(false);
        }

        private void OnDestroy()
        {
            _backdrop.onClick.RemoveListener(Close);
        }
    }
}
