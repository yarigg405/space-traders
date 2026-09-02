using Assets.Code.Common.Inventory;
using Assets.Code.Common.StaticData;
using Assets.Code.UI.Infrastructure.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.ItemInfo
{
    public sealed class ItemInfoPresenter : IPresenter<ItemInfoView>
    {
        private const string LocalizationTable = "LocalizationTable";

        private readonly IUIManager _uiManager;
        private readonly IAttributeIconsConfig _attributeIcons;

        private readonly Stack<ItemSO> _backStack = new();
        private readonly Stack<ItemSO> _forwardStack = new();

        private ItemInfoView _view;
        private LocalizedString _nameString;
        private ItemSO _current;
        private string _activeTabId;

        public ItemInfoPresenter(IUIManager uiManager, IAttributeIconsConfig attributeIcons)
        {
            _uiManager = uiManager;
            _attributeIcons = attributeIcons;
        }

        void IPresenter<ItemInfoView>.Show(ItemInfoView view, object args)
        {
            if (args is not ItemInfoArgs data || data.Item == null)
            {
                Debug.LogError("ItemInfoView needs ItemInfoArgs with an item");
                return;
            }

            _view = view;
            _backStack.Clear();
            _forwardStack.Clear();
            _current = null;
            _activeTabId = null;

            view.CloseButton.onClick.AddListener(Close);
            view.BackButton.onClick.AddListener(Back);
            view.ForwardButton.onClick.AddListener(Forward);

            foreach (var tab in view.Tabs)
            {
                if (tab == null || tab.TabButton == null)
                    continue;

                var tabId = tab.TabId;
                tab.TabButton.onClick.AddListener(() => SelectTab(tabId));
            }

            OpenItem(data.Item, pushHistory: false);
        }

        void IPresenter<ItemInfoView>.Hide(ItemInfoView view)
        {
            view.CloseButton.onClick.RemoveListener(Close);
            view.BackButton.onClick.RemoveListener(Back);
            view.ForwardButton.onClick.RemoveListener(Forward);

            foreach (var tab in view.Tabs)
                if (tab && tab.TabButton)
                    tab.TabButton.onClick.RemoveAllListeners();

            UnbindName();

            _backStack.Clear();
            _forwardStack.Clear();
            _current = null;
            _view = null;
        }

        public void OpenItem(ItemSO item) => OpenItem(item, pushHistory: true);

        private void OpenItem(ItemSO item, bool pushHistory)
        {
            if (item == null || item == _current)
                return;

            if (pushHistory && _current != null)
            {
                _backStack.Push(_current);
                _forwardStack.Clear();
            }

            _current = item;
            Render();
        }

        private void Back()
        {
            if (_backStack.Count == 0)
                return;

            _forwardStack.Push(_current);
            _current = _backStack.Pop();
            Render();
        }

        private void Forward()
        {
            if (_forwardStack.Count == 0)
                return;

            _backStack.Push(_current);
            _current = _forwardStack.Pop();
            Render();
        }

        private void Render()
        {
            if (_view == null || _current == null)
                return;

            if (_view.ItemIcon)
            {
                _view.ItemIcon.sprite = _current.Icon;
                _view.ItemIcon.enabled = _current.Icon;
            }

            BindName(_current.Id);

            if (_view.BackButton)
                _view.BackButton.interactable = _backStack.Count > 0;

            if (_view.ForwardButton)
                _view.ForwardButton.interactable = _forwardStack.Count > 0;

            RefreshTabs();
        }

        private void RefreshTabs()
        {
            ItemInfoTabView firstVisible = null;
            ItemInfoTabView active = null;

            foreach (var tab in _view.Tabs)
            {
                if (tab == null)
                    continue;

                var visible = IsTabAvailable(tab.TabId);
                tab.SetVisible(visible);

                if (!visible)
                    continue;

                firstVisible ??= tab;

                if (tab.TabId == _activeTabId)
                    active = tab;
            }

            active ??= firstVisible;
            _activeTabId = active ? active.TabId : null;

            foreach (var tab in _view.Tabs)
                if (tab)
                    tab.SetActive(tab.TabId == _activeTabId);

            BuildActiveTabContent();
        }

        private void SelectTab(string tabId)
        {
            if (_activeTabId == tabId || !IsTabAvailable(tabId))
                return;

            _activeTabId = tabId;

            foreach (var tab in _view.Tabs)
                tab.SetActive(tab.TabId == tabId);

            BuildActiveTabContent();
        }

        private bool IsTabAvailable(string tabId)
        {
            return tabId switch
            {
                ItemInfoTabIds.Attributes => true,
                _ => false,
            };
        }

        private void BuildActiveTabContent()
        {
            switch (_activeTabId)
            {
                case ItemInfoTabIds.Attributes:
                    BuildAttributes();
                    break;
            }
        }

        private void BuildAttributes()
        {
            var root = _view.AttributesRoot;
            if (root == null || _view.AttributeRowPrefab == null)
                return;

            root.ClearChildren();

            foreach (var attribute in _current.GetAttributes())
            {
                var row = Object.Instantiate(_view.AttributeRowPrefab, root);
                var icon = attribute.Icon != null ? attribute.Icon : _attributeIcons.Get(attribute.NameKey);
                row.Bind(attribute, icon);
            }
        }

        private void Close()
        {
            _uiManager.CloseModal<ItemInfoPopup>();
        }

        private void BindName(string entryKey)
        {
            UnbindName();

            _nameString = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = entryKey
            };

            _nameString.StringChanged += OnNameChanged;
            _nameString.RefreshString();
        }

        private void UnbindName()
        {
            if (_nameString == null)
                return;

            _nameString.StringChanged -= OnNameChanged;
            _nameString = null;
        }

        private void OnNameChanged(string value)
        {
            if (_view != null && _view.ItemNameTmp)
                _view.ItemNameTmp.text = value;
        }
    }
}
