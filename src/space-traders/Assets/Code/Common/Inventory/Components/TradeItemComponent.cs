using System;
using UnityEngine;


namespace Assets.Code.Common.Inventory.Components
{
    [Serializable]
    public sealed class TradeItemComponent : InventoryComponent
    {
        [field: SerializeField] public string CategoryId { get; private set; }
    }
}