using System;
using System.Collections.Generic;


namespace Assets.Code.Common.Serialization
{
    public static class ComponentIndexByType
    {
        private static Dictionary<Type, int> _cachedComponentsIndexes = new();

        public static int IndexByType(Type componentType)
        {
            if(!_cachedComponentsIndexes.ContainsKey(componentType))
                _cachedComponentsIndexes[componentType] =
                    Array.IndexOf(GameComponentsLookup.componentTypes, componentType);

            return _cachedComponentsIndexes[componentType];
        }
    }
}
