using SQLite;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("Planets")]
    public sealed class PlanetORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

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
