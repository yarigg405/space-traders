using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.DI
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        protected IContainerBuilder Builder;

        public void Install(IContainerBuilder builder)
        {
            Builder = builder;
            Install();
        }

        protected abstract void Install();
    }
}
