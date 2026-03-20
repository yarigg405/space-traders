using SQLite;
using System;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("PlayerShips")]
    public sealed class PlayerShipORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("ownerCharacterId")]
        public int OwnerCharacterId { get; set; }

        [Column("shipState")]
        public ShipState ShipState { get; set; }
    }

    [Serializable]
    public enum ShipState : ushort
    {
        Stored,         //in hangar, innactive
        Selected,
    }
}
