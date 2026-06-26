using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class PlayersRepository
    {
        private readonly IDataBaseManager _dataBase;

        public PlayersRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal PlayerORM GetOrCreatePlayer(string playerName)
        {
            var player = _dataBase.QuerySingle<PlayerORM>
                ("SELECT * FROM Players WHERE login =?"
                , playerName);

            if (player == null)
            {
                player = new PlayerORM
                {
                    PlayerLogin = playerName,
                    PlayerGuid = Guid.NewGuid().ToString(),
                };
                _dataBase.CreateNew(player);
            }

            return player;
        }
    }
}
