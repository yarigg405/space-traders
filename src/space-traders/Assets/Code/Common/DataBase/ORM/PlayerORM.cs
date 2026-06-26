using SQLite;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("Players")]
    public sealed class PlayerORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("guid")]
        public string PlayerGuid { get; set; }

        [Indexed]
        [Column("login")]
        public string PlayerLogin { get; set; }
    }
}
