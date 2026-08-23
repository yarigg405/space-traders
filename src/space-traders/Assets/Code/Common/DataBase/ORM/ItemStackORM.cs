using SQLite;
using System;


namespace Assets.Code.Common.DataBase.ORM
{
    [Table("ItemStacks")]
    public sealed class ItemStackORM
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("itemId")]
        public string ItemId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Indexed("IX_ItemStacks_ContainerOwner", 0)]
        [Column("containerType")]
        public ContainerType ContainerType { get; set; }

        [Indexed("IX_ItemStacks_ContainerOwner", 1)]
        [Column("containerId")]
        public int ContainerId { get; set; }

        [Indexed("IX_ItemStacks_ContainerOwner", 2)]
        [Column("ownerType")]
        public ItemStackOwnerType OwnerType { get; set; }

        [Indexed("IX_ItemStacks_ContainerOwner", 3)]
        [Column("ownerId")]
        public int OwnerId { get; set; }
    }

    [Serializable]
    public enum ItemStackOwnerType : byte
    {
        None = 0,
        Character = 1,
        Station = 2,
    }

    [Serializable]
    public enum ContainerType : byte
    {
        None = 0,
        ShipCargo = 1,
        StationHangar = 2,
        ContainerInSpace = 3,
    }
}
