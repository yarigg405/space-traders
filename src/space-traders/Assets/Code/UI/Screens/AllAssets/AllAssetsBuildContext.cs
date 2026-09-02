using Assets.Code.Common.Inventory;
using System;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public sealed class AllAssetsBuildContext
    {
        public AllAssetsItemRowView ItemRowPrefab { get; }
        public Action OnHierarchyChanged { get; }
        public Action<ItemSO> OnItemInfo { get; }
        public Action<AllAssetsContextRequest> OnItemContext { get; }
        public Func<int, bool> IsStationExpanded { get; }
        public Action<int, bool> OnStationExpandChanged { get; }

        public AllAssetsBuildContext(
            AllAssetsItemRowView itemRowPrefab,
            Action onHierarchyChanged,
            Action<ItemSO> onItemInfo,
            Action<AllAssetsContextRequest> onItemContext,
            Func<int, bool> isStationExpanded,
            Action<int, bool> onStationExpandChanged)
        {
            ItemRowPrefab = itemRowPrefab;
            OnHierarchyChanged = onHierarchyChanged;
            OnItemInfo = onItemInfo;
            OnItemContext = onItemContext;
            IsStationExpanded = isStationExpanded;
            OnStationExpandChanged = onStationExpandChanged;
        }
    }
}
