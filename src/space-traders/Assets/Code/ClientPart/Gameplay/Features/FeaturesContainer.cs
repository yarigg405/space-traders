using Assets.Code.Common;
using Assets.Code.Common.Time;
using System.Collections.Generic;


namespace Assets.Code.ClientPart.Gameplay.Features
{
    public sealed class FeaturesContainer
    {
        private readonly List<Feature> _features = new();
        private readonly List<Feature> _fixedUpdateFeatures = new();
        private readonly InterpolationClock _interpolation;

        public FeaturesContainer(InterpolationClock interpolation)
        {
            _interpolation = interpolation;
        }

        private float _accumulator;

        public bool IsInitialized { get; private set; }

        public void AddFeature(Feature feature)
        {
            _features.Add(feature);
        }

        public void AddFixedUpdateFeature(Feature feature)
        {
            _fixedUpdateFeatures.Add(feature);
        }

        public void Initialize()
        {
            foreach (var feature in _fixedUpdateFeatures)
                feature.Initialize();

            foreach (var feature in _features)
                feature.Initialize();

            IsInitialized = true;
        }

        public void Stop()
        {
            IsInitialized = false;

            foreach (var feature in _fixedUpdateFeatures)
            {
                feature.DeactivateReactiveSystems();
                feature.ClearReactiveSystems();
            }

            foreach (var feature in _features)
            {
                feature.DeactivateReactiveSystems();
                feature.ClearReactiveSystems();
            }
        }

        public void Cleanup()
        {
            foreach (var feature in _fixedUpdateFeatures)
            {
                feature.Cleanup();
                feature.TearDown();
            }

            foreach (var feature in _features)
            {
                feature.Cleanup();
                feature.TearDown();
            }

            _fixedUpdateFeatures.Clear();
            _features.Clear();
        }

        internal void Tick(float frameDeltaTime)
        {
            _accumulator += frameDeltaTime;
            int steps = 0;

            while (_accumulator >= GameConstants.FIXED_DELTA_TIME && steps < GameConstants.MAX_CATCHUP_TICKS)
            {
                foreach (var feature in _fixedUpdateFeatures)
                {
                    feature.Execute();
                    feature.Cleanup();
                }

                _accumulator -= GameConstants.FIXED_DELTA_TIME;
                steps++;
            }

            if (steps == GameConstants.MAX_CATCHUP_TICKS)
                _accumulator = 0f;

            _interpolation.Alpha = _accumulator / GameConstants.FIXED_DELTA_TIME;

            foreach (var feature in _features)
            {
                feature.Execute();
                feature.Cleanup();
            }
        }
    }
}