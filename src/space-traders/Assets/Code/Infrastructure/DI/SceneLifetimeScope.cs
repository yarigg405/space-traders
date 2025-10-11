using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.DI
{
    internal sealed class SceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private MonoInstaller[] _monoInstallers;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var mono in _monoInstallers)
                mono.Install(builder);
        }
    }
}
