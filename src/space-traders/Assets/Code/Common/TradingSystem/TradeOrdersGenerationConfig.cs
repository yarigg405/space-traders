using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.Common.TradingSystem
{
    [CreateAssetMenu(fileName = "TradeOrdersGenerationConfig", menuName = "ScriptableObjects/TradeOrdersGenerationConfig", order = 51)]
    public sealed class TradeOrdersGenerationConfig : ScriptableObject
    {
        [Header("Global generation parameters")]
        [Tooltip("Random +/- spread applied to BasePrice for every generated order. 0.15 = +/-15%.")]
        [Range(0f, 1f)]
        [SerializeField] private float _priceVariancePercent = 0.15f;

        [Tooltip("How much a station's BUY order sits below BasePrice (the NPC margin). 0.1 = 10% lower.")]
        [Range(0f, 1f)]
        [SerializeField] private float _buySpreadPercent = 0.1f;

        [Tooltip("Lifetime of a generated order, in hours, used to compute ExpiresAt.")]
        [Min(1)]
        [SerializeField] private int _orderLifetimeHours = 24;

        [Header("Per-item rules")]
        [SerializeField] private TradeOrderItemRule[] _rules = Array.Empty<TradeOrderItemRule>();

        public float PriceVariancePercent => _priceVariancePercent;
        public float BuySpreadPercent => _buySpreadPercent;
        public int OrderLifetimeHours => _orderLifetimeHours;

        public IReadOnlyList<TradeOrderItemRule> GetRules() => _rules;
    }

    [Serializable]
    public struct TradeOrderItemRule
    {
        [Tooltip("Must match ItemSO.Id.")]
        public string ItemId;

        [Min(0)]
        public long BasePrice;

        [Min(0)]
        public int MinQuantity;

        [Min(0)]
        public int MaxQuantity;

        [Tooltip("Station buys this item from the player.")]
        public bool AllowBuyOrders;

        [Tooltip("Station sells this item to the player.")]
        public bool AllowSellOrders;

        [Tooltip("Probability the item appears on any given station.")]
        [Range(0f, 1f)]
        public float AppearChance;
    }
}
