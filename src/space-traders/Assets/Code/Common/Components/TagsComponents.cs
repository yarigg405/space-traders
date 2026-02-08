using Assets.Code.Common.Serialization;


namespace Assets.Code.Common.Components
{
    [Game] public sealed class Ship : ISerializeComponent { }
    [Game] public sealed class Station : ISerializeComponent { }
    [Game] public sealed class StationDockingBay : ISerializeComponent { }
    [Game] public sealed class Planet : ISerializeComponent { }
}
