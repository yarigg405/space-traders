using System.Collections.Generic;


namespace Assets.Code.Gameplay
{
    public sealed class FeaturesContainer
    {
        private readonly List<Feature> _features = new();
        public bool IsInitialized { get; private set; }

        public void Add(Feature feature)
        {
            _features.Add(feature);
        }

        public void Initialize()
        {
            foreach (var feature in _features)
            {
                feature.Initialize();
            }
            IsInitialized = true;
        }

        public void Stop()
        {
            IsInitialized = false;
            foreach (var feature in _features)
            {
                feature.DeactivateReactiveSystems();
                feature.ClearReactiveSystems();
            }
        }

        public void Cleanup()
        {
            foreach (var feature in _features)
            {
                feature.Cleanup();
                feature.TearDown();
            }

            _features.Clear();
        }

        internal void Tick()
        {
            foreach (var feature in _features)
            {
                feature.Execute();
                feature.Cleanup();
            }
        }
    }
}
