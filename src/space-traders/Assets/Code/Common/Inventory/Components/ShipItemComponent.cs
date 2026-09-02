using System;
using System.Collections.Generic;

namespace Assets.Code.Common.Inventory.Components
{
    [Serializable]
    public sealed class ShipItemComponent : InventoryComponent
    {
        public string PrefabName;

        public float MaxCargo;

        public override IEnumerable<ItemAttribute> GetAttributes()
        {
            yield return new ItemAttribute(ItemAttributeKeys.MaxCargo,
                AttributeValueFormat.Format(ItemAttributeKeys.VolumeValueFormat, MaxCargo));
        }
    }
}
