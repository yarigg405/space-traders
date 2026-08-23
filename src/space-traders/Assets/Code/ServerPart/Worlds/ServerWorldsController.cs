using System.Collections.Generic;
using VContainer.Unity;


namespace Assets.Code.ServerPart.Worlds
{
    public sealed class ServerWorldsController : ITickable
    {
        private readonly EcsWorldsBuilder _worldsBuilder;
        private readonly EcsWorldDestroyer _destroyer;

        private readonly Dictionary<string, EcsWorldInstance> _scenesWorldsDict;


        public ServerWorldsController(EcsWorldsBuilder worldsBuilder)
        {
            _worldsBuilder = worldsBuilder;
            _destroyer = new();

            _scenesWorldsDict = new();
        }

        void ITickable.Tick()
        {
            foreach (var world in _scenesWorldsDict.Values)
            {
                world.Feature.Execute();
                world.Feature.Cleanup();
            }
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
