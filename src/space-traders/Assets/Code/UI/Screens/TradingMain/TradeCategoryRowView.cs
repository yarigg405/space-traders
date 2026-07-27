using Assets.Code.Common.TradingSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradeCategoryRowView : MonoBehaviour
    {
        private const string LocalizationTable = "LocalizationTable";

        [SerializeField] private Button _headerButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private RectTransform _expandArrow;
        [SerializeField] private Transform _childrenRoot;

        private readonly List<TradeCategoryRowView> _childCategories = new();
        private readonly List<TradeItemRowView> _itemRows = new();

        private LocalizedString _labelString;
        private TradeCategoryBuildContext _context;
        private TradeItemCategory _category;
        private bool _hasChildren;
        private bool _expanded;

        public void Bind(TradeItemCategory category, TradeCategoryBuildContext context)
        {
            _category = category;
            _context = context;

            BindLabel(category.LocalizationKey);

            if (_icon)
            {
                _icon.sprite = category.Icon;
                _icon.enabled = category.Icon;
            }

            BuildChildCategories();
            BuildItems();

            HasContent = ComputeHasContent();
            _hasChildren = HasContent;

            if (_expandArrow)
                _expandArrow.gameObject.SetActive(_hasChildren);

            SetExpanded(_context.ExpandByDefault);

            _headerButton.onClick.AddListener(OnHeaderClicked);

            gameObject.SetActive(HasContent);
        }

        public bool HasContent { get; private set; }

        private void BuildChildCategories()
        {
            if (_category.SubCategories == null) return;

            foreach (var subCategory in _category.SubCategories)
            {
                var row = Instantiate(_context.CategoryRowPrefab, _childrenRoot);
                row.Bind(subCategory, _context);
                _childCategories.Add(row);
            }
        }

        private void BuildItems()
        {
            if (!_context.ItemsByCategory.TryGetValue(_category.FullCategoryId, out var items))
                return;

            foreach (var item in items)
            {
                var row = Instantiate(_context.ItemRowPrefab, _childrenRoot);
                row.Bind(item, _context.OnItemSelected);
                _itemRows.Add(row);
            }
        }

        private bool ComputeHasContent()
        {
            if (_itemRows.Count > 0)
                return true;

            foreach (var child in _childCategories)
            {
                if (child.HasContent)
                    return true;
            }

            return false;
        }

        private void OnHeaderClicked()
        {
            if (_hasChildren)
                SetExpanded(!_expanded);
        }

        private void SetExpanded(bool expanded)
        {
            _expanded = expanded && _hasChildren;

            if (_childrenRoot)
                _childrenRoot.gameObject.SetActive(_expanded);

            if (_expandArrow)
                _expandArrow.localRotation = Quaternion.Euler(0f, 0f, _expanded ? 180f : 270f);

            _context.OnHierarchyChanged?.Invoke();
        }

        private void BindLabel(string entryKey)
        {
            UnbindLabel();

            _labelString = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = entryKey
            };

            _labelString.StringChanged += OnLabelChanged;
            _labelString.RefreshString();
        }

        private void UnbindLabel()
        {
            if (_labelString == null)
                return;

            _labelString.StringChanged -= OnLabelChanged;
            _labelString = null;
        }

        private void OnLabelChanged(string value)
        {
            _label.text = value;
        }

        private void OnDestroy()
        {
            UnbindLabel();
            _headerButton.onClick.RemoveListener(OnHeaderClicked);
        }
    }
}
