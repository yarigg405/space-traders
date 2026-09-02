using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.Common.Inventory
{
    [Serializable]
    public sealed class ItemComponents
    {
        [SerializeReference] private List<object> _components = new();

        public void AddComponent(object component)
        {
            _components.Add(component);
        }

        public IEnumerable<ItemAttribute> GetAttributes()
        {
            foreach (var component in _components)
            {
                if (component is InventoryComponent inventoryComponent)
                {
                    foreach (var attribute in inventoryComponent.GetAttributes())
                        yield return attribute;
                }
            }
        }

        public bool HasComponent<T>()
        {
            foreach (var component in _components)
            {
                if (component is T)
                {
                    return true;
                }
            }

            return false;
        }

        public T GetComponent<T>()
        {
            foreach (var component in _components)
            {
                if (component is T tComponent)
                {
                    return tComponent;
                }
            }

            throw new Exception($"Component of type {typeof(T).Name} is not found!");
        }

        public bool TryGetComponent<T>(out T tComponent)
        {
            foreach (var component in _components)
            {
                if (component is T tComp)
                {
                    tComponent = tComp;
                    return true;
                }
            }

            tComponent = default(T);
            return false;
        }
    }
}
