using Assets.Code.Infrastructure.DI;
using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.Layers;
using Assets.Code.UI.LoadingScreens;
using Assets.Code.UI.Screens;
using Assets.Code.UI.Screens.GameMain;
using Assets.Code.UI.Screens.GameSceneScreens.GameSceneMain;
using Assets.Code.UI.Screens.MenuScene;
using Assets.Code.UI.Screens.MessageBox;
using Assets.Code.UI.Screens.StationScreens;
using Assets.Code.UI.Screens.TradingMain;
using UnityEngine;
using VContainer;
using Assets.Code.UI.Screens.CurrentShipInfo;
using Assets.Code.UI.Screens.Wallet;
using Assets.Code.UI.Screens.BuySellPopups;


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
            Builder.Register<NavigationIntentFactory>(Lifetime.Transient).AsImplementedInterfaces();
            Builder.Register<ScreenViewsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            Builder.Register<UIManager>(Lifetime.Singleton).AsImplementedInterfaces();

            RegisterMenuScreens();
            RegisterGameScreens();
        }

        private void RegisterMenuScreens()
        {
            Builder.Register<MainMenuMainScreen>(Lifetime.Singleton);
            Builder.Register<MainMenuMainPresenter>(Lifetime.Transient);

            Builder.Register<SelectCharacterScreen>(Lifetime.Singleton);
            Builder.Register<SelectCharacterPresenter>(Lifetime.Transient);
            Builder.Register<SelectCharacterIntent>(Lifetime.Transient);

            Builder.Register<CreateCharacterScreen>(Lifetime.Singleton);
            Builder.Register<CreateCharacterPresenter>(Lifetime.Transient);

            Builder.Register<ClientConnectionScreen>(Lifetime.Singleton);
            Builder.Register<ClientConnectionPresenter>(Lifetime.Transient);

            Builder.Register<StationMainScreen>(Lifetime.Singleton);
            Builder.Register<StationMainPresenter>(Lifetime.Transient);
        }

        private void RegisterGameScreens()
        {
            Builder.Register<RequestDockPopup>(Lifetime.Singleton);
            Builder.Register<RequestDockPopupPresenter>(Lifetime.Transient);

            Builder.Register<TradingMainPopup>(Lifetime.Singleton);
            Builder.Register<TradingMainPresenter>(Lifetime.Transient);

            Builder.Register<BuyItemPopup>(Lifetime.Singleton);
            Builder.Register<BuyItemPresenter>(Lifetime.Transient);

            Builder.Register<AwaitServerResponsePopup>(Lifetime.Singleton);

            Builder.Register<MessageBoxPopup>(Lifetime.Singleton);
            Builder.Register<MessageBoxPresenter>(Lifetime.Transient);

            Builder.Register<GameSceneMainScreen>(Lifetime.Singleton);
            Builder.Register<GameSceneMainPresenter>(Lifetime.Transient);

            Builder.Register<GameMainScreen>(Lifetime.Singleton);
            Builder.Register<GameMainPresenter>(Lifetime.Transient);

            Builder.Register<WalletScreen>(Lifetime.Singleton);
            Builder.Register<WalletPresenter>(Lifetime.Transient);

            Builder.Register<CurrentShipInfoScreen>(Lifetime.Singleton);
            Builder.Register<CurrentShipInfoPresenter>(Lifetime.Transient);
        }
    }
}
