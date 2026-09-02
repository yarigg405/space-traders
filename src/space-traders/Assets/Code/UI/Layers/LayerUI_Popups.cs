using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace Assets.Code.UI.Layers
{
    public sealed class LayerUI_Popups : LayerUI
    {
        [SerializeField] private int _baseSortingOrder = 0;

        private readonly List<RaycastResult> _raycastResults = new();

        public override void ShowView(UIScreenView view)
        {
            base.ShowView(view);
            ReorderSorting();
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null || !mouse.leftButton.wasPressedThisFrame)
                return;

            if (transform.childCount < 2)
                return;

            BringClickedPopupToFront(mouse.position.ReadValue());
        }

        private void BringClickedPopupToFront(Vector2 screenPosition)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var pointerData = new PointerEventData(eventSystem) { position = screenPosition };

            _raycastResults.Clear();
            eventSystem.RaycastAll(pointerData, _raycastResults);

            if (_raycastResults.Count == 0)
                return;

            var popup = FindPopupRoot(_raycastResults[0].gameObject.transform);
            if (popup == null)
                return;

            popup.SetAsLastSibling();
            ReorderSorting();
        }

        private void ReorderSorting()
        {
            var order = _baseSortingOrder;

            for (int i = 0; i < transform.childCount; i++)
            {
                var canvas = transform.GetChild(i).GetComponent<Canvas>();
                if (canvas == null)
                    continue;

                canvas.overrideSorting = true;
                canvas.sortingOrder = order;
                order++;
            }
        }

        private Transform FindPopupRoot(Transform hit)
        {
            var current = hit;

            while (current != null)
            {
                if (current.parent == transform)
                    return current;

                current = current.parent;
            }

            return null;
        }
    }
}
