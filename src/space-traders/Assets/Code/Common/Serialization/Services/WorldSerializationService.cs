using Assets.Code.Common.Serialization.Data;
using Assets.Code.Common.Serialization.Extensions;
using Assets.Code.ServerPart.Worlds;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Code.Common.Serialization.Services
{
    public sealed class WorldSerializationService
    {
        public string SerializeGameWorld(EcsWorldInstance world)
        {
            var snapshots = world.Contexts.game.GetEntities()
                  .Where(x => x.GetComponents().Any(c => c is ISerializeComponent))
                  .Select(e => e.AsSerializedEntity())
                  .ToList();

            var json = JsonSerializator.ToJson(snapshots);
            return json;
        }

        public void FillContext(string json, GameContext context)
        {
            var snapshots = JsonSerializator.FromJson<List<EntitySnapshot>>(json);
            foreach (var snapshot in snapshots)
            {
                context.CreateEntity()
                    .FillEntityWith(snapshot);
            }
        }
    }
}