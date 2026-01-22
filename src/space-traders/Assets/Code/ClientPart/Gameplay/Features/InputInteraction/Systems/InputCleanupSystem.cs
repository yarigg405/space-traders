using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class InputCleanupSystem : ICleanupSystem
    {
        private readonly IGroup<InputEntity> _entities;
        private readonly List<InputEntity> _buffer = new(8);

        public InputCleanupSystem(InputContext input)
        {
            _entities = input.GetGroup(InputMatcher.Input);
        }

        void ICleanupSystem.Cleanup()
        {
            foreach (var entity in _entities.GetEntities(_buffer))
            {
                entity.Destroy();
            }
        }
    }
}
