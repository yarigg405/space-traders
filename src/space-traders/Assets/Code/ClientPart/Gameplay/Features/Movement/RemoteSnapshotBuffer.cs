using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement
{
    public sealed class RemoteSnapshotBuffer
    {
        private const int MaxPerEntity = 32;
        private readonly Dictionary<uint, List<Snapshot>> _buffers = new();

        public void Add(uint id, uint tick, double2 position, float rotation)
        {
            if (!_buffers.TryGetValue(id, out var list))
                _buffers[id] = list = new(MaxPerEntity);

            if (list.Count > 0 && tick <= list[list.Count - 1].Tick) return;

            list.Add(new Snapshot
            {
                Tick = tick,
                Pos = position,
                Rot = rotation
            });
            if (list.Count > MaxPerEntity) list.RemoveAt(0);
        }

        public bool TryGet(uint id, float renderTick, out double2 pos, out float rot)
        {
            pos = default; rot = default;
            if (!_buffers.TryGetValue(id, out var list) || list.Count == 0) return false;

            for (int i = list.Count - 1; i > 0; i--)
            {
                if (list[i - 1].Tick <= renderTick && renderTick <= list[list.Count - 1].Tick)
                {
                    var a = list[i - 1]; var b = list[i];
                    float t = (renderTick - a.Tick) / (b.Tick - a.Tick);
                    pos = math.lerp(a.Pos, b.Pos, (double)t);
                    rot = Mathf.LerpAngle(a.Rot, b.Rot, t);
                    return true;
                }
            }
            
            var edge = renderTick >= list[list.Count-1].Tick ? list[list.Count - 1] : list[0];
            pos = edge.Pos; rot = edge.Rot;
            return true;
        }

        public void Remove(uint id)
        {
            _buffers.Remove(id);
        }


        private struct Snapshot
        {
            public uint Tick;
            public double2 Pos;
            public float Rot;
        }
    }
}