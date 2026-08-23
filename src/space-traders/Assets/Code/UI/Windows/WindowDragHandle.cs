using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Code.UI.Windows
{
    public sealed class WindowDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform _draggableRoot;

        private Canvas _canvas;

        private void Awake()
        {
            if (_draggableRoot == null)
                _draggableRoot = (RectTransform)transform;

            _canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _draggableRoot.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var scaleFactor = _canvas != null ? _canvas.scaleFactor : 1f;
            _draggableRoot.anchoredPosition += eventData.delta / scaleFactor;
        }
    }
}
