using Unity.Mathematics;
using Yrr.Utils;


namespace Assets.Code.ClientPart.Visual.Player
{
    public sealed class PlayerViewModel
    {
        public ReactiveValue<int2> PlayerQuadrant = new();
        public ReactiveValue<bool> PlayerIsWarping = new();

        public void ManualUpdatePlayerModel(GameEntity entity)
        {
            PlayerQuadrant.Value = entity.QuadrantIndex;
            PlayerIsWarping.Value = entity.isWarping;
        }
    }
}