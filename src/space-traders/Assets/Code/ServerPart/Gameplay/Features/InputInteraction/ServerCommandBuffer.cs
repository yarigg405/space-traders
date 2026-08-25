using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction
{
    public sealed class ServerCommandBuffer
    {
        private const int Size = 128;
        private readonly Dictionary<uint, (uint tick, Vector2 input)[]> _rings = new();

        public void Store(uint entityId, uint tick, Vector2 input)
        {
            if (!_rings.TryGetValue(entityId, out var ring))
                _rings[entityId] = ring = new (uint, Vector2)[Size];
            ring[tick % Size] = (tick, input);
        }

        public bool TryConsume(uint entityId, uint tick, out Vector2 input)
        {
            input = default;
            if (!_rings.TryGetValue(entityId, out var ring)) return false;
            var slot  = ring[tick % Size];
            if (slot.tick != tick) return false;

            input = slot.input;
            return true;        
        }

        public void Remove(uint entityId)
        {
            _rings.Remove(entityId);
        }
    }
}
