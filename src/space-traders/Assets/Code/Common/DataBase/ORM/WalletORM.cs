using SQLite;
using System;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("Wallets")]
    public sealed class WalletORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Indexed("IX_Wallets_Owner", 0)]
        [Column("ownerType")]
        public WalletOwnerType OwnerType { get; set; }

        [Indexed("IX_Wallets_Owner", 1)]
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
