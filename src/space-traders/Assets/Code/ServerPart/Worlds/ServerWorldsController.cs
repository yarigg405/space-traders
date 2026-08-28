using Assets.Code.Common;
using Assets.Code.Common.Time;
using System.Collections.Generic;
using VContainer.Unity;


namespace Assets.Code.ServerPart.Worlds
{
    public sealed class ServerWorldsController : ITickable
    {
        private readonly EcsWorldsBuilder _worldsBuilder;
        private readonly EcsWorldDestroyer _destroyer;
        private readonly TickCounter _tickCounter;

        private readonly Dictionary<string, EcsWorldInstance> _scenesWorldsDict;
        private float _accumulator;


        public ServerWorldsController(EcsWorldsBuilder worldsBuilder, TickCounter tickCounter)
        {
            _worldsBuilder = worldsBuilder;
            _destroyer = new();

            _scenesWorldsDict = new();
            _tickCounter = tickCounter;
        }

        void ITickable.Tick()
        {
            _accumulator += UnityEngine.Time.deltaTime;

            int steps = 0;
            while (_accumulator >= GameConstants.FIXED_DELTA_TIME && steps < GameConstants.MAX_CATCHUP_TICKS)
            {
                _tickCounter.Tick();

                foreach (var world in _scenesWorldsDict.Values)
                {
                    world.Feature.Execute();
                    world.Feature.Cleanup();
                }

                _accumulator -= GameConstants.FIXED_DELTA_TIME;
                steps++;
            }

            if (steps == GameConstants.MAX_CATCHUP_TICKS)
                _accumulator = 0f;
        }

        public EcsWorldInstance GetOrCreateWorld(string sceneName)
        {
            if (!_scenesWorldsDict.ContainsKey(sceneName))
            {
                _scenesWorldsDict[sceneName] = _worldsBuilder.CreateNewServerWorld(sceneName);
            }

            return _scenesWorldsDict[sceneName];
        }

        public void DestroyWorld(string sceneName)
        {
            _destroyer.DestroyWorld(_scenesWorldsDict[sceneName]);
            _scenesWorldsDict.Remove(sceneName);
        }
    }
}
