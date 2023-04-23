using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Base.Utils
{
    public static class NewtonJsonSerializerSettings
    {
        public static JsonSerializerSettings CamelIgnoreNullOutput = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),

            }
        };
        public static readonly JsonSerializerSettings SNAKE = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public static readonly JsonSerializerSettings CAMEL = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        public static readonly JsonSerializerSettings AUTO = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static readonly JsonSerializerSettings INCLUDE_INHERITED_PROPS = new JsonSerializerSettings()
        {
            ContractResolver = new IncludeInheritedPropResolverContract()
        };
    }

    public class IncludeInheritedPropResolverContract : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            prop.Writable = true;
            return prop;
        }
    }
}
