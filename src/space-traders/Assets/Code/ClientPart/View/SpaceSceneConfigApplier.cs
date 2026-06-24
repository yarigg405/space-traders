using Assets.Code.Infrastructure.Loading;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.View
{
    internal sealed class SpaceSceneConfigApplier : IStartable
    {
        private readonly SpaceSceneDataHolder _sceneDataHolder;

        public SpaceSceneConfigApplier(SpaceSceneDataHolder sceneDataHolder)
        {
            _sceneDataHolder = sceneDataHolder;
        }

        void IStartable.Start()
        {
            var config = _sceneDataHolder.Current.ConfigJson;
            Debug.Log($"[SpaceScene] config: {config}");
        }
    }
}
