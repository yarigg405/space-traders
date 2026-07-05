using System;
using UnityEngine;
using Yrr.Utils;


namespace Assets.Code.Common.TradingSystem
{
    [Serializable]
    public struct TradeItemCategory
    {
        public string Id;        
        public string LocalizationKey;
        public Sprite Icon;

        [ReadOnly]
        public string FullCategoryId;

        public TradeItemCategory[] SubCategories;
    }
}
