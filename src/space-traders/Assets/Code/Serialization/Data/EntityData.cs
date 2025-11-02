using Newtonsoft.Json;
using System.Collections.Generic;


namespace Assets.Code.Serialization.Data
{
    public sealed class EntityData
    {
        [JsonProperty("es")]
        public List<EntitySnapshot> GameEntitySnapshots;
    }
}
