using SQLite;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("SellOrders")]
    public sealed class SellOrderORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public long Id { get; set; }

        [Column("itemId")]
        public string ItemId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price")]
        public long Price { get; set; }

        [Column("stationId")]
        public int StationId { get; set; }

        [Column("expiresAt")]
        public long ExpiresAt { get; set; }

        [Column("sellerId")]
        public int SellerId { get; set; }
    }
}
