using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace iTalk.API {
    /// <summary>
    /// SignalR Contract Resolver
    /// 序列化時將 Json 屬性名稱首字改為小寫
    /// </summary>
    public class SignalRContractResolver : IContractResolver {
        readonly Assembly assembly;
        readonly IContractResolver camelCaseContractResolver;
        readonly IContractResolver defaultContractSerializer;

        /// <summary>
        /// 建構函數
        /// </summary>
        public SignalRContractResolver() {
            defaultContractSerializer = new DefaultContractResolver();
            camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            assembly = typeof(Connection).Assembly;
        }

        /// <summary>
        /// 自己的型別用小寫序列化...
        /// </summary>
        /// <param name="type">要序列化的型別</param>
        /// <returns>JsonContract</returns>
        public JsonContract ResolveContract(Type type) {
            if (type.Assembly.Equals(assembly)) {
                return defaultContractSerializer.ResolveContract(type);
            }

            return camelCaseContractResolver.ResolveContract(type);
        }
    }
}