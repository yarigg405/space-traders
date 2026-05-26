using SQLite;
using System;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("CharacterLocations")]
    public sealed class CharacterLocationORM
    {
        [PrimaryKey]
        [Column("characterId")]
        public int CharacterId { get; set; }

        [Column("locationType")]
        public LocationType LocationType { get; set; }

        [Column("locationId")]
        public int CurrentLocationId { get; set; }      //star system id, space station id etc...

        [Column("dockBayId")]
        public int DockBayId { get; set; }


        [Column("positionX")]
        public double PositionX { get; set; }

        [Column("positionY")]
        public double PositionY { get; set; }
    }

    [Serializable]
    public enum LocationType : ushort
    {
        None,
        Space,
        Station,
    }
}
