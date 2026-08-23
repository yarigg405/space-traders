using SQLite;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("Characters")]
    public sealed class CharacterORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Indexed]
        [Column("playerId")]
        public int PlayerId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("currentShipId")]
        public int CurrentShipId { get; set; }
    }
}
