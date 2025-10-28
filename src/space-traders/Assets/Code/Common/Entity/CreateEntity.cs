namespace Assets.Code.Common.Entity
{
    public static class CreateEntity
    {
        public static GameEntity Empty(Contexts contexts) =>
          contexts.game.CreateEntity();
    }
}
