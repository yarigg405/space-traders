using Unity.Mathematics;
using Yrr.Utils;


namespace Assets.Code.ClientPart.Visual.Player
{
    public sealed class PlayerQuadrantChangeObserver
    {
        public ReactiveValue<int2> PlayerQuadrant = new();

        public void ManualUpdatePlayerQuadrant(int2 playerQuadrant)
        {
            PlayerQuadrant.Value = playerQuadrant;
        }
    }
}