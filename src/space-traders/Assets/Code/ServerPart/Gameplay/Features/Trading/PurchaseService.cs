using Assets.Code.Common.DataBase;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Networking;


namespace Assets.Code.ServerPart.Gameplay.Features.Trading
{
    public sealed class PurchaseService
    {
        private readonly SellOrdersRepository _sellOrders;
        private readonly WalletsRepository _wallets;
        private readonly ItemStacksRepository _itemStacks;
        private readonly IDataBaseManager _dataBase;

        public PurchaseService(SellOrdersRepository sellOrders, WalletsRepository wallets,
            ItemStacksRepository itemStacks, IDataBaseManager dataBase)
        {
            _sellOrders = sellOrders;
            _wallets = wallets;
            _itemStacks = itemStacks;
            _dataBase = dataBase;
        }

        public bool TryBuyFromSellOrder(int characterId, long orderId, int quantity, out string error)
        {
            var order = _sellOrders.GetById(orderId);
            if (order == null)
            {
                error = ErrorCodes.OrderNotFound;
                return false;
            }

            if (quantity < 1 || quantity > order.Quantity)
            {
                error = ErrorCodes.OrderNotFound;
                return false;
            }

            long total = order.Price * quantity;

            if (_wallets.GetCharacterMoney(characterId) < total)
            {
                error = ErrorCodes.NotEnoughMoney;
                return false;
            }

            _dataBase.RunInTransaction(_ =>
            {
                _wallets.ChangeMoney(characterId, -total);
                _wallets.ChangeMoney(order.SellerId, total);
                _sellOrders.SetQuantity(order.Id, order.Quantity - quantity);
                _itemStacks.CreateStationStack(order.ItemId, quantity, order.StationId, characterId);
            });

            error = null;
            return true;
        }
    }
}
