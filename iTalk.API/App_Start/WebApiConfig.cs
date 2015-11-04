using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace iTalk.API {
    /// <summary>
    /// Web API 設置
    /// </summary>
    public static class WebApiConfig {
        /// <summary>
        /// 註冊 Web API 設置
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new GlobalExceptionFilterAttribute());

            // Web API 設定和服務
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi 2",
                routeTemplate: "{controller}"
                //defaults: new { userName = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi 1",
            //    routeTemplate: "{controller}/{friendName}",
            //    defaults: new { friendName = RouteParameter.Optional }
            //);

            // 移除 Xml 序列化器
            config.Formatters.Remove(config.Formatters.FirstOrDefault(f => f.GetType() == typeof(XmlMediaTypeFormatter)));
            var jsonFormatter = config.Formatters.JsonFormatter;
            // Web API Json 序列化時首字小寫，日期為 UTC 格式
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }
    }
}
