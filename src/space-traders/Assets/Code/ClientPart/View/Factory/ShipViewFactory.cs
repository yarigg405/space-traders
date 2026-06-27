using Assets.Code.ClientPart.AssetManagement;
using Assets.Code.Common.Inventory.Components;
using Assets.Code.Common.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace Assets.Code.ClientPart.View.Factory
{
    internal sealed class ShipViewFactory : IShipViewFactory
    {
        private readonly IObjectResolver _instantiator;
        private readonly IAssetProvider _assetProvider;
        private readonly IItemsCatalog _itemsCatalog;

        public ShipViewFactory(IObjectResolver instantiator, IAssetProvider assetProvider, IItemsCatalog itemsCatalog)
        {
            _instantiator = instantiator;
            _assetProvider = assetProvider;
            _itemsCatalog = itemsCatalog;
        }

        GameObject IShipViewFactory.CreateShipModel(string shipModelId, Transform parent)
        {
            var item = _itemsCatalog.GetItem(shipModelId);
            var shipComponent = item.Components.GetComponent<ShipItemComponent>();
            var prefab = _assetProvider.LoadAsset<EntityBehaviour>(shipComponent.PrefabName);

            return _instantiator.Instantiate(prefab.gameObject, parent);
        }
    }
}
