using Assets.Code.ClientPart.AssetManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Yrr.Utils;


namespace Assets.Code.ClientPart.View.Factory
{
    internal sealed class EntityViewFactory : IEntityViewFactory
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
            var view = _instantiator.Instantiate(prefab,
                _farAwayPos.GetRandomCoordinatesAroundPointZX(15f), Quaternion.identity);

            entity.AddViewModel(new());
            view.SetEntity(entity);

            return view;
        }

        EntityBehaviour IEntityViewFactory.CreateViewForEntityFromPrefab(GameEntity entity)
        {
            EntityBehaviour prefab = entity.ViewPrefab;
            var view = _instantiator.Instantiate(prefab,
                _farAwayPos.GetRandomCoordinatesAroundPointZX(15f), Quaternion.identity);

            entity.AddViewModel(new());
            view.SetEntity(entity);

            return view;
        }
    }
}
