using Assets.Code.ClientPart.View;


namespace Assets.Code.ClientPart
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
