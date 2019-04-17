using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BWCore.Common
{
    /// <summary>
    /// 序列化忽略属性
    /// </summary>
    public class JsonPropsIgnoreContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var ignoreNames = new List<string>();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.GetCustomAttributes(typeof(JsonPropIgnoreAttibute), true).Any())
                {
                    ignoreNames.Add(pi.Name);
                }
            }

            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            return list.Where(p =>
            {
                return !ignoreNames.Contains(p.PropertyName);
            }).ToList();
        }
    }
}
