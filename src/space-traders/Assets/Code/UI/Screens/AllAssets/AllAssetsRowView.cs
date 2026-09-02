using Assets.Code.Common.Inventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public sealed class AllAssetsRowView : MonoBehaviour
    {
        [SerializeField] private Button _headerButton;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private RectTransform _expandArrow;
        [SerializeField] private Transform _childrenRoot;

        private readonly List<AllAssetsItemRowView> _itemRows = new();

        private AllAssetsBuildContext _context;
        private AllAssetsStationModel _station;
        private bool _hasContent;
        private bool _expanded;

        public bool HasContent => _hasContent;

        public void Bind(AllAssetsStationModel station, AllAssetsBuildContext context)
        {
            _station = station;
            _context = context;

            _label.text = string.IsNullOrEmpty(station.SystemName)
                ? station.StationName
                : $"{station.SystemName} — {station.StationName}";

            BuildItems();

            _hasContent = _itemRows.Count > 0;
            SetExpanded(_context.IsStationExpanded != null && _context.IsStationExpanded(station.StationId));
            _headerButton.onClick.AddListener(OnHeaderClicked);
            gameObject.SetActive(_hasContent);
        }

        private void BuildItems()
        {
            if (_station.Items == null)
                return;

            foreach (var model in _station.Items)
            {
                var row = Instantiate(_context.ItemRowPrefab, _childrenRoot);
                row.Bind(model.Item, model.Amount, _context.OnItemInfo, OnItemRightClicked);
                _itemRows.Add(row);
            }
        }

        private void OnItemRightClicked(ItemSO item, int amount, Vector2 position)
        {
            _context.OnItemContext?.Invoke(new AllAssetsContextRequest(
                item, amount, _station.StationId, _station.StationName, _station.SystemName, position));
        }

        private void OnHeaderClicked()
        {
            if (!_hasContent)
                return;

            SetExpanded(!_expanded);
            _context.OnStationExpandChanged?.Invoke(_station.StationId, _expanded);
        }

        private void SetExpanded(bool expanded)
        {
            _expanded = expanded && _hasContent;
            _childrenRoot.gameObject.SetActive(_expanded);
            _expandArrow.localRotation = Quaternion.Euler(0f, 0f, _expanded ? 180f : 270f);
            _context.OnHierarchyChanged?.Invoke();
        }

        private void OnDestroy()
        {
            _headerButton.onClick.RemoveListener(OnHeaderClicked);
        }
    }
}
