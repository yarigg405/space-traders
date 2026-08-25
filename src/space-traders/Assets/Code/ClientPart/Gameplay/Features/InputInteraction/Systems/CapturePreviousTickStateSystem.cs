using Entitas;
using System;
using System.Collections.Generic;
using System.Text;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class CapturePreviousTickStateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public CapturePreviousTickStateSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.GlobalPosition);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
//                entity.ReplaPre
            }
        }
    }
}
