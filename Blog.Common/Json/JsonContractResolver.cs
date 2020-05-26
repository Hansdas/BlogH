using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blog.Common.Json
{
    /// <summary>
    /// 针对领域模型的属性重写Newtonsoft的反序列化规则
    /// </summary>
    /// <remarks>
    /// 可以直接在属性上标记JsonProperty属性，领域模型都标记代码量大，也可以使用
    /// var contractResolver = new DefaultContractResolver()
    /// contractResolver.DefaultMembersSearchFlags |= BindingFlags.NonPublic;
    /// var settings = new JsonSerializerSettings
    /// {
    ///    ContractResolver = contractResolver
    /// };
    /// </remarks>
    public class JsonContractResolver : DefaultContractResolver
    {
        public JsonSerializerSettings SetJsonSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new JsonContractResolver()
            };
            return jsonSerializerSettings;
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty.Writable)
                return jsonProperty;
            var propertyInfo = member as PropertyInfo;
            if (jsonProperty == null)
                return jsonProperty;
            var privateSetter = propertyInfo.GetSetMethod(true) != null;
            jsonProperty.Writable = privateSetter;
            return jsonProperty;
        }
    }
}
