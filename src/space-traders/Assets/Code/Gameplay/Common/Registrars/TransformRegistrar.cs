using Assets.Code.View;


namespace Assets.Code.Gameplay.Common.Registrars
{
    internal sealed class TransformRegistrar : EntityComponentRegistrar
    {
        public override void RegisterComponents()
        {
            Entity.AddTransform(transform);
        }

        public override void UnRegisterComponents()
        {
            Entity.RemoveTransform();
        }
    }
}
