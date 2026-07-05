using System.Collections.Generic;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Common.TradingSystem
{
    [CreateAssetMenu(fileName = "TradeItemsCategoryConfig", menuName = "ScriptableObjects/TradeItemsCategoryConfig", order = 51)]
    public sealed class TradeItemsCategoryConfig : ScriptableObject
    {
        [SerializeField] private TradeItemCategory[] _categories;
        [SerializeField] private Dictionary<string, TradeItemCategory> _cachedCategories;

        public void UpdateCategoryIds()
        {
            _cachedCategories = new();
            for (int i = 0; i < _categories.Length; i++)
            {
                UpdateCategoryId(string.Empty, ref _categories[i]);
            }
        }

        private void UpdateCategoryId(string parentCategory, ref TradeItemCategory category)
        {
            var fullName = parentCategory.IsNulOrEmpty() ?
                category.Id :
                $"{parentCategory}.{category.Id}";

            category.FullCategoryId = fullName;
            _cachedCategories[fullName] = category;

            for (int i = 0; i < category.SubCategories.Length; i++)
            {
                UpdateCategoryId(fullName, ref category.SubCategories[i]);
                _cachedCategories[fullName] = category;
            }
        }

        public IEnumerable<TradeItemCategory> GetAllCategories()
        {
            return _categories;
        }
    }
}
