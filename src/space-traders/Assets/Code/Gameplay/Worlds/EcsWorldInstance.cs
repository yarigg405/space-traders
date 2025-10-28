using Assets.Code.Gameplay.Features;


namespace Assets.Code.Gameplay.Worlds
{
    public sealed class EcsWorldInstance
    {
        public string SceneName { get; private set; }
        public ServerGameFeature Feature { get; private set; }
        public Contexts Contexts { get; private set; }

        public EcsWorldInstance(string sceneName, ServerGameFeature feature, Contexts contexts)
        {
            SceneName = sceneName;
            Feature = feature;
            Contexts = contexts;
        }
    }
}
