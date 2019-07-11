using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.AspNet
{
    internal class OriginalNameCamelCaseContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly string[] _namespaces;

        public OriginalNameCamelCaseContractResolver(Type[] types)
            : this(types.Select(t => t.Namespace).ToArray())
        {
        }

        public OriginalNameCamelCaseContractResolver(string[] namespaces)
        {
            _namespaces = namespaces;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = base.CreateProperties(type, memberSerialization);
            if (!_namespaces.Any(x => type.Namespace.StartsWith(x)))
            {
                return list;
            }

            foreach (var prop in list)
            {
                prop.PropertyName = NamingStrategy.GetPropertyName(prop.UnderlyingName, false);
            }

            return list;
        }
    }
}