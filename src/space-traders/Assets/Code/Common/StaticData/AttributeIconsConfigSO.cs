using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.Common.StaticData
{
    [CreateAssetMenu(fileName = "AttributeIconsConfig", menuName = "ScriptableObjects/AttributeIconsConfigSO", order = 51)]
    public sealed class AttributeIconsConfigSO : ScriptableObject, IAttributeIconsConfig
    {
        [SerializeField] private Dictionary<string, Sprite> Icons = new();

        public Sprite Get(string attributeKey)
        {
            if (string.IsNullOrEmpty(attributeKey))
                return null;

            return Icons.TryGetValue(attributeKey, out var icon) ? icon : null;
        }
    }
}
