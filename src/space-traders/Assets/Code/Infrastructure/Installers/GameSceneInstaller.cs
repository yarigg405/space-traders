using Assets.Code.Gameplay.InputInteraction;
using Assets.Code.Infrastructure.DI;
using VContainer;


namespace Assets.Code.Infrastructure.Installers
{
    internal sealed class GameSceneInstaller : MonoInstaller
    {
        private IContainerBuilder _builder;

        public override void Install(IContainerBuilder builder)
        {
            _builder = builder;

            RegisterInput();
        }

        private void RegisterInput()
        {
            _builder.Register<CameraRaycaster>(Lifetime.Scoped).AsSelf();
            _builder.Register<MouseClickNotificator>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
            _builder.Register<InputListener>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
        }
    }
}
