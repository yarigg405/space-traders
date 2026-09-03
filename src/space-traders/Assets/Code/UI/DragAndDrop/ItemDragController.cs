using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Code.UI.DragAndDrop
{
    public sealed class ItemDragController
    {
        private const string GhostPrefabPath = "UI/Elements/DragGhost";

        private readonly List<RaycastResult> _raycastResults = new();

        private DragGhostView _prefab;
        private DragGhostView _ghost;
        private ItemDragPayload _payload;

        public void Begin(ItemDragPayload payload, Sprite icon, Vector2 screenPosition)
        {
            EnsureGhost();

            _payload = payload;

            if (_ghost != null)
                _ghost.Show(icon, screenPosition);
        }

        public void Move(Vector2 screenPosition)
        {
            if (_ghost != null)
                _ghost.Move(screenPosition);
        }

        public bool End(Vector2 screenPosition)
        {
            var accepted = TryDrop(screenPosition);

            if (_ghost != null)
                _ghost.Hide();

            return accepted;
        }

        private bool TryDrop(Vector2 screenPosition)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return false;

            var pointerData = new PointerEventData(eventSystem) { position = screenPosition };

            _raycastResults.Clear();
            eventSystem.RaycastAll(pointerData, _raycastResults);

            foreach (var result in _raycastResults)
            {
                var target = result.gameObject.GetComponentInParent<IItemDropTarget>();
                if (target != null && target.CanAccept(_payload))
                {
                    target.OnItemDropped(_payload);
                    return true;
                }
            }

            return false;
        }

        private void EnsureGhost()
        {
            if (_ghost != null)
                return;

            if (_prefab == null)
                _prefab = Resources.Load<DragGhostView>(GhostPrefabPath);

            if (_prefab == null)
            {
                Debug.LogError($"DragGhostView prefab was not found at Resources/{GhostPrefabPath}");
                return;
            }

            _ghost = Object.Instantiate(_prefab);
        }
    }
}
