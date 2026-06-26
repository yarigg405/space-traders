using SQLite;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("StarSystems")]
    public sealed class StarSystemORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Indexed]
        [Column("name")]
        public string Name { get; set; }

        [Column("positionX")]
        public float PositionX { get; set; }

        [Column("positionY")]
        public float PositionY { get; set; }

        [Column("sceneName")]
        public string SceneName { get; set; }

        [Column("skybox")]
        public int Skybox { get; set; }

        [Column("lightSettings")]
        public int LightSettings { get; set; }
    }
}
