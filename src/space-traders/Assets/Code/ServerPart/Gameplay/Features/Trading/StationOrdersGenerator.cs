using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Common.Time;
using Assets.Code.Common.TradingSystem;
using System;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Trading
{
    public sealed class StationOrdersGenerator
    {
        private readonly SpaceStationsRepository _stations;
        private readonly BuyOrdersRepository _buyOrders;
        private readonly SellOrdersRepository _sellOrders;
        private readonly TradeOrdersGenerationConfig _config;
        private readonly ITimeService _time;
        private readonly IDataBaseManager _dataBase;

        private readonly System.Random _random = new();

        public StationOrdersGenerator(SpaceStationsRepository stations,
            BuyOrdersRepository buyOrders, SellOrdersRepository sellOrders,
            TradeOrdersGenerationConfig config, ITimeService time, IDataBaseManager dataBase)
        {
            _stations = stations;
            _buyOrders = buyOrders;
            _sellOrders = sellOrders;
            _config = config;
            _time = time;
            _dataBase = dataBase;
        }

        public void GenerateForAllStations()
        {
            if (_config == null)
            {
                Debug.LogError($"{nameof(StationOrdersGenerator)}: {nameof(TradeOrdersGenerationConfig)} is not assigned.");
                return;
            }

            var stations = _stations.GetAll();
            var rules = _config.GetRules();
            long expiresAt = new DateTimeOffset(_time.UtcNow).ToUnixTimeSeconds()
                + _config.OrderLifetimeHours * 3600L;

            _dataBase.RunInTransaction(_ =>
            {
                _buyOrders.DeleteAllNpcOrders();
                _sellOrders.DeleteAllNpcOrders();

                foreach (var station in stations)
                {
                    foreach (var rule in rules)
                    {
                        if (string.IsNullOrEmpty(rule.ItemId))
                            continue;

                        if (_random.NextDouble() > rule.AppearChance)
                            continue;

                        int quantity = RollQuantity(rule);
                        if (quantity <= 0)
                            continue;

                        if (rule.AllowSellOrders)
                        {
                            _sellOrders.Insert(new SellOrderORM
                            {
                                ItemId = rule.ItemId,
                                Quantity = quantity,
                                Price = RollSellPrice(rule.BasePrice),
                                StationId = station.Id,
                                ExpiresAt = expiresAt,
                                SellerId = SellOrdersRepository.NpcOwnerId,
                            });
                        }

                        if (rule.AllowBuyOrders)
                        {
                            _buyOrders.Insert(new BuyOrderORM
                            {
                                ItemId = rule.ItemId,
                                Quantity = quantity,
                                Price = RollBuyPrice(rule.BasePrice),
                                StationId = station.Id,
                                ExpiresAt = expiresAt,
                                BuyerId = BuyOrdersRepository.NpcOwnerId,
                            });
                        }
                    }
                }
            });
        }

        private int RollQuantity(TradeOrderItemRule rule)
        {
            int min = Mathf.Max(0, rule.MinQuantity);
            int max = Mathf.Max(min, rule.MaxQuantity);
            return _random.Next(min, max + 1);
        }

        private long RollSellPrice(long basePrice)
        {
            return ApplyVariance(basePrice);
        }

        private long RollBuyPrice(long basePrice)
        {
            long buyBase = (long)Math.Round(basePrice * (1.0 - _config.BuySpreadPercent));
            return ApplyVariance(buyBase);
        }

        private long ApplyVariance(long price)
        {
            double variance = (_random.NextDouble() * 2.0 - 1.0) * _config.PriceVariancePercent;
            long result = (long)Math.Round(price * (1.0 + variance));
            return Math.Max(1, result);
        }
    }
}
