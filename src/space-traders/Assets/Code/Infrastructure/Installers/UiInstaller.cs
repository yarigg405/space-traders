using Assets.Code.Infrastructure.DI;
using Assets.Yrr.UI.UI_System;
using UnityEngine;
using VContainer;
using Yrr.UI;
using Yrr.UI.Infrastructure;


namespace Assets.Code.Infrastructure.Installers
{
    internal class UiInstaller : MonoInstaller
    {
        [SerializeField] private Transform _screensRoot;

        public override void Install(IContainerBuilder builder)
        {
            var screens = _screensRoot.GetComponentsInChildren<IUIScreen>(true);
            var supplier = new ScreensSupplier(screens);
            var uiManager = new UIManager(supplier);

            foreach (var screen in screens)
            {
                screen.SetupUiManager(uiManager);
                screen.Hide();
            }

            builder.RegisterInstance(uiManager).AsSelf().AsImplementedInterfaces();
        }
    }
}
