using UnityEngine;


namespace Assets.Code.Common.Inventory
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/ItemSO", order = 51)]
    public class ItemSO : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public float Mass { get; private set; }
        [field: SerializeField] public float Volume { get; private set; }
        [field: SerializeField] public ItemComponents Components { get; private set; }
        [HideInInspector] public ItemFlags Flags { get; private set; }

        internal void AddComponent(object component)
        {
            Components.AddComponent(component);
        }

        private void OnValidate()
        {
            Flags = Components.ToFlags();
        }
    }

    internal static class ItemFlagsGenerator
    {
        internal static ItemFlags ToFlags(this ItemComponents components)
        {
            var flags = 0;
            return (ItemFlags)flags;
        }
    }
}
