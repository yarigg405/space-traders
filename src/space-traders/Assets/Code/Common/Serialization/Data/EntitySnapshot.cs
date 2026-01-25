using Newtonsoft.Json;
using System.Collections.Generic;


namespace Assets.Code.Common.Serialization.Data
{
    public sealed class EntitySnapshot
    {
        [JsonProperty("c")]
        public List<ISerializeComponent> Components;

        [JsonProperty("r")]
        public List<int> ComponentsForRemoving = new();
    }
}
