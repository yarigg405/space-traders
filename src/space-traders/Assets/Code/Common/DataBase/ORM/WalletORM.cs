using SQLite;
using System;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("Wallets")]
    public sealed class WalletORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("walletId")]
        public int WalletId { get; set; }

        [Column("ownerType")]
        public WalletOwnerType OwnerType { get; set; }

        [Column("ownerId")]
        public int OwnerId { get; set; }

        [Column("money")]
        public long Money { get; set; }
    }

    [Serializable]
    public enum WalletOwnerType : byte
    {
        None = 0,
        Character = 1,
    }
}
