using UnityEngine;


namespace Assets.Code.UI.Screens.StationStorage
{
    public sealed class StationItemsStorageView : UIScreenView
    {
        [field: SerializeField] public Transform TilesRoot { get; private set; }
        [field: SerializeField] public StationItemTileView TilePrefab { get; private set; }
    }
}
