using System;
using Assets.Code.UI.Screens.StationScreens;


namespace Assets.Code.Infrastructure.Loading
{
    public sealed class StationSceneDataHolder
    {
        private LoadStationData _current;

        public LoadStationData Current
        {
            get => _current;
            set
            {
                _current = value;
                Changed?.Invoke();
            }
        }

        public event Action Changed;
    }
}
