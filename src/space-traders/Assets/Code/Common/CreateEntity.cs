namespace Assets.Code.Common
{
    public static class CreateEntity
    {
        public static GameEntity Empty(Contexts contexts) =>
          contexts.game.CreateEntity();
    }
}
