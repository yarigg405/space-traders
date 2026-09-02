using Assets.Code.Common.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public sealed class AllAssetsView : UIScreenView
    {
        [Header("Stations tree")]
        [SerializeField] private RectTransform _stationsRoot;
        [SerializeField] private AllAssetsRowView _stationRowPrefab;
        [SerializeField] private AllAssetsItemRowView _itemRowPrefab;

        private readonly List<AllAssetsRowView> _rows = new();
        private readonly HashSet<int> _expandedStations = new();

        public event Action<ItemSO> ItemInfoRequested;
        public event Action<AllAssetsContextRequest> ItemContextRequested;

        public void Setup(IReadOnlyList<AllAssetsStationModel> stations)
        {
            ClearRows();

            var context = new AllAssetsBuildContext(
                _itemRowPrefab,
                RebuildLayout,
                RaiseItemInfo,
                RaiseItemContext,
                IsStationExpanded,
                OnStationExpandChanged);

            foreach (var station in stations)
            {
                var row = Instantiate(_stationRowPrefab, _stationsRoot);
                row.Bind(station, context);
                _rows.Add(row);
            }

            RebuildLayout();
        }

        private bool IsStationExpanded(int stationId) => _expandedStations.Contains(stationId);

        private void OnStationExpandChanged(int stationId, bool expanded)
        {
            if (expanded)
                _expandedStations.Add(stationId);
            else
                _expandedStations.Remove(stationId);
        }

        private void RaiseItemInfo(ItemSO item) => ItemInfoRequested?.Invoke(item);

        private void RaiseItemContext(AllAssetsContextRequest request) => ItemContextRequested?.Invoke(request);

        private void RebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_stationsRoot);
        }

        private void ClearRows()
        {
            _rows.Clear();
            _stationsRoot.ClearChildren();
        }
    }
}
