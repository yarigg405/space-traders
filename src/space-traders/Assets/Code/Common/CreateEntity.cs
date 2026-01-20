namespace Assets.Code.Common
{
    public static class CreateEntity
    {
        public static GameEntity Empty(Contexts contexts) =>
          contexts.game.CreateEntity();

        public static InputEntity EmptyInput(Contexts contexts) =>
            contexts.input.CreateEntity();

        public static MetaEntity EmptyMeta(Contexts contexts) =>
            contexts.meta.CreateEntity();
    }
}
