using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


namespace Assets.Code.ClientPart.UI.Elements.Navigation
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

                var screenPoint = camera.WorldToScreenPoint(world);

                if (screenPoint.z < 0f)
                {
                    screenPoint.x = Screen.width - screenPoint.x;
                    screenPoint.y = Screen.height - screenPoint.y;
                }

                screenPoint.x = Mathf.Clamp(screenPoint.x, _edgeMargin, Screen.width - _edgeMargin);
                screenPoint.y = Mathf.Clamp(screenPoint.y, _edgeMargin, Screen.height - _edgeMargin);

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvasRect, screenPoint, _uiCamera, out var localPoint))
                {
                    pair.Value.Rect.anchoredPosition = localPoint;
                }
            }
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
