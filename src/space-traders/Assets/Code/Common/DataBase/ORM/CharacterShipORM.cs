using SQLite;
using System;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("PlayerShips")]
    public sealed class CharacterShipORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("ownerCharacterId")]
        public int OwnerCharacterId { get; set; }


        [Column("shipModelId")]
        public string ShipModelId { get; set; }

        [Column("shipFitJson")]
        public string ShipFitJson { get; set; }

        [Column("shipState")]
        public ShipState ShipState { get; set; }
    }

    [Serializable]
    public enum ShipState : byte
    {
        None = 0,
        Stored = 1,         //in hangar, inactive
        Selected = 2,
    }
}
