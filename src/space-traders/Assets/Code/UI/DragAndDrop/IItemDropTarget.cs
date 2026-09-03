namespace Assets.Code.UI.DragAndDrop
{
    public interface IItemDropTarget
    {
        bool CanAccept(ItemDragPayload payload);
        void OnItemDropped(ItemDragPayload payload);
    }
}
