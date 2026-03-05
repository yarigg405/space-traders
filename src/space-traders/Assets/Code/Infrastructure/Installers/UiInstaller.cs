using Assets.Code.Infrastructure.DI;
using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.LoadingScreens;
using Assets.Code.UI.Screens;
using UnityEngine;
using VContainer;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class UiInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoadingScreen _sceneLoadingScreen;
        [SerializeField] private LayerUI_Screens _layerScreen;
        [SerializeField] private LayerUI_HUD _layerOverlay;
        [SerializeField] private LayerUI_Popups _layerPopups;


        protected override void Install()
        {
            Builder.RegisterInstance(_sceneLoadingScreen).AsSelf();
            Builder.RegisterInstance(_layerScreen).AsSelf();
            Builder.RegisterInstance(_layerOverlay).AsSelf();
            Builder.RegisterInstance(_layerPopups).AsSelf();

            Builder.Register<ScreensProvider>(Lifetime.Transient).AsImplementedInterfaces();
            Builder.Register<ScreenViewsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<UIManager>(Lifetime.Singleton);

            RegisterScreens();
        }

        private void RegisterScreens()
        {
            Builder.Register<RequestDockPopup>(Lifetime.Singleton);
            Builder.Register<RequestDockPopupPresenter>(Lifetime.Transient);
        }
    }
}
