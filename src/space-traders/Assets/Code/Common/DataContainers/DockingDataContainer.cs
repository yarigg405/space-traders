using System;


namespace Assets.Code.Common.DataContainers
{
    [Serializable]
    public struct DockingDataContainer
    {
        // short names for json optimization
        public uint StId;   //station id
        public uint Dbid;   //docking bay id
    }
}
