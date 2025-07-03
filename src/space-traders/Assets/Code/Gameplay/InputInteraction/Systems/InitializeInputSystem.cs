using Assets.Code.Common.Entity;
using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.InputInteraction.Systems
{
    public sealed class InitializeInputSystem : IInitializeSystem
    {
        void IInitializeSystem.Initialize()
        {
            CreateEntity.Empty()
                .AddClickedPosition(Vector3.zero)
                .isInput = true;
        }
    }
}
