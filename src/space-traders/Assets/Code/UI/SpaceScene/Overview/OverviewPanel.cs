using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Common.Time;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


namespace Assets.Code.UI.SpaceScene.Overview
{
    public sealed class OverviewPanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private OverviewRow _rowPrefab;
        [SerializeField] private float _distanceRefreshInterval = 0.5f;
        [SerializeField] private float _farDistanceRefreshInterval = 5f;

        [Inject] private readonly NavigationRegistry _registry;
        [Inject] private readonly SelectionService _selectionService;
        [Inject] private readonly IPlayerProvider _playerProvider;
        [Inject] private readonly ITimeService _time;

        private readonly Dictionary<GameEntity, RowState> _rows = new();


        private void OnEnable()
        {
            foreach (var entity in _registry.Objects)
                AddRow(entity);

            _registry.Added += AddRow;
            _registry.Removed += RemoveRow;
        }

        private void OnDisable()
        {
            _registry.Added -= AddRow;
            _registry.Removed -= RemoveRow;

            ClearRows();
        }

        private void Update()
        {
            var player = _playerProvider.PlayerEntity;
            if (player == null || !player.hasGlobalPosition) return;

            var playerPosition = player.GlobalPosition;
            var deltaTime = _time.DeltaTime;

            foreach (var pair in _rows)
            {
                var state = pair.Value;

                state.Timer -= deltaTime;
                if (state.Timer > 0) continue;

                if (pair.Key.TryGetUiDistance(playerPosition, out var distance))
                {
                    state.View.SetDistance(DistanceFormat.Format(distance));
                    state.Timer = distance >= DistanceFormat.MetersPerAu
                        ? _farDistanceRefreshInterval
                        : _distanceRefreshInterval;
                }
                else
                {
                    state.Timer = _distanceRefreshInterval;
                }
            }
        }

        private void AddRow(GameEntity entity)
        {
            if (_rows.ContainsKey(entity)) return;

            var row = Instantiate(_rowPrefab, _content);
            row.Bind(entity, _selectionService.Select);
            _rows.Add(entity, new RowState { View = row });
        }

        private void RemoveRow(GameEntity entity)
        {
            if (!_rows.TryGetValue(entity, out var state)) return;

            if (state.View != null)
                Destroy(state.View.gameObject);

            _rows.Remove(entity);
        }

        private void ClearRows()
        {
            foreach (var pair in _rows)
            {
                if (pair.Value.View != null)
                    Destroy(pair.Value.View.gameObject);
            }

            _rows.Clear();
        }

        private sealed class RowState
        {
            public OverviewRow View;
            public float Timer;
        }
    }
}
