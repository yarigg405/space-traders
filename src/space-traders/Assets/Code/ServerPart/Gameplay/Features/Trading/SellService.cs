using Assets.Code.Common.DataBase;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Networking;


namespace Assets.Code.ServerPart.Gameplay.Features.Trading
{
    public sealed class SellService
    {
        private readonly BuyOrdersRepository _buyOrders;
        private readonly WalletsRepository _wallets;
        private readonly ItemStacksRepository _itemStacks;
        private readonly IDataBaseManager _dataBase;

        public SellService(BuyOrdersRepository buyOrders, WalletsRepository wallets,
            ItemStacksRepository itemStacks, IDataBaseManager dataBase)
        {
            _buyOrders = buyOrders;
            _wallets = wallets;
            _itemStacks = itemStacks;
            _dataBase = dataBase;
        }

        public bool TrySellToBuyOrder(int characterId, long orderId, int quantity, out string error)
        {
            var order = _buyOrders.GetById(orderId);
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

            if (_itemStacks.GetOwnedAmount(characterId, order.StationId, order.ItemId) < quantity)
            {
                error = ErrorCodes.NotEnoughItems;
                return false;
            }

            long total = order.Price * quantity;

            _dataBase.RunInTransaction(_ =>
            {
                _wallets.ChangeMoney(characterId, total);
                _wallets.ChangeMoney(order.BuyerId, -total);
                _buyOrders.SetQuantity(order.Id, order.Quantity - quantity);
                _itemStacks.RemoveFromStation(characterId, order.StationId, order.ItemId, quantity);
            });

            error = null;
            return true;
        }
    }
}
