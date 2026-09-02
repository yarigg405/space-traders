using UnityEngine;


namespace Assets.Code.Common.Inventory
{
    public readonly struct ItemAttribute
    {
        public readonly string NameKey;
        public readonly string Value;
        public readonly Sprite Icon;

        public ItemAttribute(string nameKey, string value, Sprite icon = null)
        {
            NameKey = nameKey;
            Value = value;
            Icon = icon;
        }
    }
}
