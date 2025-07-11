using Assets.Code.Infrastructure.AssetManagement;
using VContainer;
using VContainer.Unity;
using UnityEngine;


namespace Assets.Code.View.Factories
{
    public sealed class EntityViewFactory : IEntityViewFactory
    {
        private readonly IObjectResolver _instantiator;
        private readonly IAssetProvider _assetProvider;
        private readonly Vector3 _farAwayPos = new Vector3(-99999, -99999, -99999);

        public EntityViewFactory(IObjectResolver instantiator, IAssetProvider assetProvider)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }

        EntityBehaviour IEntityViewFactory.CreateViewForEntityFromPath(GameEntity entity)
        {
            EntityBehaviour prefab = _assetProvider.LoadAsset<EntityBehaviour>(entity.ViewPath);
            var view = _instantiator.Instantiate<EntityBehaviour>(prefab, _farAwayPos, Quaternion.identity);
            view.SetEntity(entity);

            return view;
        }

        EntityBehaviour IEntityViewFactory.CreateViewForEntityFromPrefab(GameEntity entity)
        {
            EntityBehaviour prefab = entity.ViewPrefab;
            var view = _instantiator.Instantiate<EntityBehaviour>(prefab, _farAwayPos, Quaternion.identity);
            view.SetEntity(entity);

            return view;
        }
    }
}
