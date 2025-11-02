using Assets.Code.Serialization.Extensions;
using Newtonsoft.Json;


namespace Assets.Code.Serialization
{
    public static class JsonSerializator
    {
        private static JsonSerializerSettings _serializationSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ContractResolver = UnityMathematicsFieldsOnlyResolver.Instance,
        };

        private static JsonSerializerSettings _deserializationSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ContractResolver = UnityMathematicsFieldsOnlyResolver.Instance,
        };

        public static string ToJson(object self)
        {
            return JsonConvert.SerializeObject(self, _serializationSettings);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _deserializationSettings);
        }
    }
}
