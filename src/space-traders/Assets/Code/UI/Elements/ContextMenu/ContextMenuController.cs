using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.UI.Elements
{
    public sealed class ContextMenuController
    {
        private const string PrefabPath = "UI/Screens/ContextMenuView";

        private ContextMenuView _prefab;
        private ContextMenuView _instance;

        public void Open(Vector2 screenPosition, IReadOnlyList<ContextMenuEntry> entries)
        {
            EnsureInstance();

            if (_instance == null)
                return;

            _instance.Open(screenPosition, entries);
        }

        public void Close()
        {
            if (_instance != null)
                _instance.Close();
        }

        private void EnsureInstance()
        {
            if (_instance != null)
                return;

            if (_prefab == null)
                _prefab = Resources.Load<ContextMenuView>(PrefabPath);

            if (_prefab == null)
            {
                Debug.LogError($"ContextMenuView prefab was not found at Resources/{PrefabPath}");
                return;
            }

            _instance = Object.Instantiate(_prefab);
        }
    }
}
