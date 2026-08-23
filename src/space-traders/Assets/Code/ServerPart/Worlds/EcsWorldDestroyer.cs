using Cysharp.Threading.Tasks;


namespace Assets.Code.ServerPart.Worlds
{
    internal sealed class EcsWorldDestroyer
    {
        public void DestroyWorld(EcsWorldInstance world)
        {
            DestroyWorldAsync(world).Forget();
        }

        private async UniTask DestroyWorldAsync(EcsWorldInstance world)
        {
            world.Feature.DeactivateReactiveSystems();
            world.Feature.ClearReactiveSystems();

            foreach (var entity in world.Contexts.game.GetEntities())
            {
                entity.isDestructed = true;
            }

            await UniTask.DelayFrame(1);

            world.Feature.Cleanup();
            world.Feature.TearDown();
        }
    }
}
