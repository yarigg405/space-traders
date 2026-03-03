using SQLite;
using System;


namespace Assets.Code._Tempo
{
    [Serializable]
    public sealed class GameData
    {
        [PrimaryKey]
        public int SaveSlotId { get; set; }

        public int Points { get; set; } = 0;
    }
}
