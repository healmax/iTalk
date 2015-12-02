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
            config.MapHttpAttributeRoutes();

            //config.MapODataServiceRoute("chat", "odata", GetEdmModel());
            //config.EnableCaseInsensitive(true);

            // Web API 設定和服務
            // Web API 路由
            config.Routes.MapHttpRoute(
                name: "Default Api route",
                routeTemplate: "{controller}"
            );

            // 移除 Xml 序列化器
            config.Formatters.Remove(config.Formatters.FirstOrDefault(f => f.GetType() == typeof(XmlMediaTypeFormatter)));
            var jsonFormatter = config.Formatters.JsonFormatter;
            // Web API Json 序列化時首字小寫，日期為 UTC 格式
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //jsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new GlobalExceptionFilterAttribute());
            config.Filters.Add(new AuthorizeAttribute());
        }

        ///// <summary>
        ///// 取得 Edm Model
        ///// </summary>
        ///// <returns>Edm Model</returns>
        //static IEdmModel GetEdmModel() {
        //    ODataModelBuilder builder = new ODataConventionModelBuilder();
        //    var types = new[]{
        //        builder.EntitySet<Dialog>("Chat").EntityType,
        //        builder.EntitySet<Dialog>("Group_Chat").EntityType,
        //    };

        //    foreach (var t in types) {
        //        t.HasKey(c => c.Id);
        //        t.Ignore(c => c.RelationId);
        //        t.Ignore(c => c.Relationship);
        //        t.Ignore(c => c.SenderId);
        //        t.Ignore(c => c.TimeStamp);
        //    }

        //    builder.EntitySet<Dialog>("Chat").EntityType.DerivesFrom<Chat>();
        //    builder.EntitySet<Dialog>("Group_Chat").EntityType.DerivesFrom<Chat>();
        //    builder.EntitySet<FileMessage>("Chat").EntityType.DerivesFrom<Chat>();
        //    builder.EntitySet<FileMessage>("Group_Chat").EntityType.DerivesFrom<Chat>();

        //    return builder.GetEdmModel();
        //}
    }
}
