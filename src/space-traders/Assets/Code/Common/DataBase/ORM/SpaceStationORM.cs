using SQLite;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("SpaceStations")]
    public sealed class SpaceStationORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Indexed]
        [Column("starSystemId")]
        public int StarSystemId { get; set; }

        [Column("prefabName")]
        public string PrefabName { get; set; }

        [Column("positionX")]
        public double PositionX { get; set; }

        [Column("positionY")]
        public double PositionY { get; set; }
    }
}
