using System.Collections.Generic;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public readonly struct AllAssetsStationModel
    {
        public readonly int StationId;
        public readonly string StationName;
        public readonly string SystemName;
        public readonly IReadOnlyList<AllAssetsItemModel> Items;

        public AllAssetsStationModel(int stationId, string stationName, string systemName,
            IReadOnlyList<AllAssetsItemModel> items)
        {
            StationId = stationId;
            StationName = stationName;
            SystemName = systemName;
            Items = items;
        }
    }
}
