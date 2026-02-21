using Unity.Mathematics;
using Yrr.Utils;


namespace Assets.Code.ClientPart.View
{
    public sealed class ViewModel
    {
        public ReactiveValue<int2> QuadrantIndex = new();
        public ReactiveValue<bool> IsWarping = new();

        public void UpdateModel(GameEntity entity)
        {
            QuadrantIndex.Value = entity.QuadrantIndex;
            IsWarping.Value = entity.isWarping;
        }
    }
}
