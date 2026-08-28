using System;

namespace Assets.Code.Common.Inventory.Components
{
    [Serializable]
    public sealed class ShipItemComponent : InventoryComponent
    {
        public string PrefabName;

        public float MaxCargo;
    }
}
