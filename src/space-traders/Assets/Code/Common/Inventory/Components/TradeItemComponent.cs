using UnityEngine;


namespace Assets.Code.Common.Inventory.Components
{
    public sealed class TradeItemComponent : InventoryComponent
    {
        [field: SerializeField] public string CategoryId { get; private set; }
    }
}