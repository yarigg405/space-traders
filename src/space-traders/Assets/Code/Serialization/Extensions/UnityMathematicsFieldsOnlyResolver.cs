using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Assets.Code.Serialization.Extensions
{
    public sealed class UnityMathematicsFieldsOnlyResolver : DefaultContractResolver
    {
        public static readonly IContractResolver Instance = new UnityMathematicsFieldsOnlyResolver();

        private UnityMathematicsFieldsOnlyResolver()
        {
            NamingStrategy = new DefaultNamingStrategy();
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            if (type.Namespace != null && type.Namespace.StartsWith("Unity.Mathematics"))
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                var props = new List<JsonProperty>(fields.Length);
                foreach (var f in fields)
                {
                    var jp = base.CreateProperty(f, memberSerialization);
                    jp.Readable = true;
                    jp.Writable = true;

                    if (f.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                        jp.Ignored = true;

                    props.Add(jp);
                }
                return props;
            }

            return base.CreateProperties(type, memberSerialization);
        }
    }
}
