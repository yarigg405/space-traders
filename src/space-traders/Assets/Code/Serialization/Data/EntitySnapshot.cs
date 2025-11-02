using Newtonsoft.Json;
using System.Collections.Generic;


namespace Assets.Code.Serialization.Data
{
    public sealed class EntitySnapshot
    {
        [JsonProperty("c")]
        public List<ISerializeComponent> Components;
    }
}
