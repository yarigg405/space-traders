using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.UI.Infrastructure.Impl
{
    public sealed class ScreenViewsProvider : IScreenViewsProvider
    {
        private const string _prefabsPath = "UI/Screens/";
        private readonly Dictionary<Type, UIScreenView> _prefabs = new();
        private readonly Dictionary<Type, UIScreenView> _cachedScreens = new();

        public ScreenViewsProvider()
        {
            var allPrefabs = Resources.LoadAll<UIScreenView>(_prefabsPath);
            foreach (var prefab in allPrefabs)
            {
                var type = prefab.GetType();
                _prefabs.Add(type, prefab);
            }
        }

        public TView GetView<TView>() where TView : UIScreenView
        {
            var type = typeof(TView);
            if (!ScreenIsCached(type))
                _cachedScreens[type] = CreateScreen<TView>();

            return _cachedScreens[type] as TView;
        }

        private bool ScreenIsCached(Type screenType)
        {
            if (!_cachedScreens.ContainsKey(screenType))
                return false;
            if (_cachedScreens[screenType] == null) return false;

            return true;
        }

        private UIScreenView CreateScreen<TView>() where TView : UIScreenView
        {
            var type = typeof(TView);
            var prefab = (TView)_prefabs[type];
            return GameObject.Instantiate(prefab);
        }
    }
}
