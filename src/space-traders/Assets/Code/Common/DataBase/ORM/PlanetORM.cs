using SQLite;
using System;


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

        [Indexed]
        [Column("starSystemId")]
        public int StarSystemId { get; set; }

        [Column("prefabName")]
        public string PrefabName { get; set; }

        [Column("positionX")]
        public double PositionX { get; set; }

        [Column("positionY")]
        public double PositionY { get; set; }

        [Column("radius")]
        public int PlanetRadius { get; set; }

        [Column("planetType")]
        public PlanetType PlanetType { get; set; }
    }

    [Serializable]
    public enum PlanetType : byte
    {
        None = 0,
        Default = 1,
        Empty = 2,

        Acid = 3,
        Desert = 4,
        Carbon = 5,
        Water = 6,
        GasGiant = 7,
    }
}
