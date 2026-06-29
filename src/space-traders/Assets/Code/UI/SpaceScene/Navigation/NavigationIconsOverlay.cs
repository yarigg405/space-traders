using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


namespace Assets.Code.UI.SpaceScene.Navigation
{
    public sealed class NavigationIconsOverlay : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private NavigationIcon _iconPrefab;
        [SerializeField] private float _edgeMargin = 40f;

        [Inject] private readonly NavigationRegistry _registry;
        [Inject] private readonly SelectionService _selectionService;
        [Inject] private readonly IPlayerProvider _playerProvider;

        private Camera _camera;
        private readonly Dictionary<GameEntity, NavigationIcon> _icons = new();


        private void OnEnable()
        {
            foreach (var entity in _registry.Objects)
                AddIcon(entity);

            _registry.Added += AddIcon;
            _registry.Removed += RemoveIcon;
            _selectionService.SelectionChanged += OnSelectionChanged;
        }

        private void OnDisable()
        {
            _registry.Added -= AddIcon;
            _registry.Removed -= RemoveIcon;
            _selectionService.SelectionChanged -= OnSelectionChanged;

            ClearIcons();
        }

        private void LateUpdate()
        {
            var player = _playerProvider.PlayerEntity;
            if (player == null || !player.hasQuadrantIndex) return;

            var camera = GetCamera();
            if (camera == null) return;

            var quadrant = player.QuadrantIndex;
            var quadrantSize = GameConstants.GAME_SCENE_QUADRANT_SIZE;

            foreach (var pair in _icons)
            {
                if (pair.Key.hasQuadrantIndex)
                {
                    var objectQuadrant = pair.Key.QuadrantIndex;
                    if (objectQuadrant.x == quadrant.x && objectQuadrant.y == quadrant.y)
                    {
                        if (pair.Value.gameObject.activeSelf)
                            pair.Value.gameObject.SetActive(false);

                        continue;
                    }
                }

                if (!pair.Key.TryGetCoordinate(out var coordinate))
                {
                    pair.Value.gameObject.SetActive(false);
                    continue;
                }

                if (!pair.Value.gameObject.activeSelf)
                    pair.Value.gameObject.SetActive(true);

                var world = new Vector3(
                    (float)(coordinate.x - quadrant.x * quadrantSize),
                    0f,
                    (float)(coordinate.y - quadrant.y * quadrantSize));

                var screenPosition = ResolveScreenPosition(camera, world);

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvasRect, screenPosition, _uiCamera, out var localPoint))
                {
                    pair.Value.Rect.anchoredPosition = localPoint;
                }
            }
        }

        private Vector2 ResolveScreenPosition(Camera camera, Vector3 world)
        {
            var screenPoint = camera.WorldToScreenPoint(world);

            var center = new Vector2(Screen.width, Screen.height) * 0.5f;
            var min = new Vector2(_edgeMargin, _edgeMargin);
            var max = new Vector2(Screen.width - _edgeMargin, Screen.height - _edgeMargin);

            var point = new Vector2(screenPoint.x, screenPoint.y);
            var behind = screenPoint.z < 0f;

            if (behind)
                point = 2f * center - point;

            var onScreen = !behind
                && point.x >= min.x && point.x <= max.x
                && point.y >= min.y && point.y <= max.y;

            if (onScreen)
                return point;

            var direction = point - center;
            if (direction.sqrMagnitude < 0.0001f)
                direction = Vector2.down;

            var halfExtents = (max - min) * 0.5f;
            var scaleX = halfExtents.x / Mathf.Abs(direction.x);
            var scaleY = halfExtents.y / Mathf.Abs(direction.y);
            var scale = Mathf.Min(scaleX, scaleY);

            return center + direction * scale;
        }

        private void OnSelectionChanged(GameEntity selected)
        {
            foreach (var pair in _icons)
                pair.Value.SetSelected(pair.Key == selected);
        }

        private void AddIcon(GameEntity entity)
        {
            if (_icons.ContainsKey(entity)) return;

            var icon = Instantiate(_iconPrefab, _canvasRect);

            var rect = icon.Rect;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            icon.Bind(entity, _selectionService.Select);
            icon.SetSelected(_selectionService.Selected == entity);
            _icons.Add(entity, icon);
        }

        private void RemoveIcon(GameEntity entity)
        {
            if (!_icons.TryGetValue(entity, out var icon)) return;

            if (icon != null)
                Destroy(icon.gameObject);

            _icons.Remove(entity);
        }

        private Camera GetCamera()
        {
            if (_camera == null)
                _camera = Camera.main;

            return _camera;
        }

        private void ClearIcons()
        {
            foreach (var pair in _icons)
            {
                if (pair.Value != null)
                    Destroy(pair.Value.gameObject);
            }

            _icons.Clear();
        }
    }
}
