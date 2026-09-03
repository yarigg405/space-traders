using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.DragAndDrop
{
    public sealed class DragGhostView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Image _icon;

        private void Awake()
        {
            Hide();
        }

        public void Show(Sprite icon, Vector2 screenPosition)
        {
            if (_icon)
            {
                _icon.sprite = icon;
                _icon.enabled = icon;
            }

            if (_root)
                _root.SetActive(true);

            Move(screenPosition);
        }

        public void Move(Vector2 screenPosition)
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

        public void Hide()
        {
            if (_root)
                _root.SetActive(false);
        }
    }
}
