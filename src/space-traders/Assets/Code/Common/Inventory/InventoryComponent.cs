using System;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Common.Inventory
{
    [Serializable]
    public abstract class InventoryComponent
    {
        [ReadOnly]
        [SerializeField] private string _type;

        public InventoryComponent()
        {
            _type = GetType().Name;
        }
    }
}
