using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace iTalk.API {
    public static class WebApiConfig {
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
                routeTemplate: "{controller}/{userName}",
                defaults: new { userName = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi 1",
            //    routeTemplate: "{controller}/{friendName}",
            //    defaults: new { friendName = RouteParameter.Optional }
            //);

            config.Formatters.Remove(config.Formatters.FirstOrDefault(f => f.GetType() == typeof(XmlMediaTypeFormatter)));
            config.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}
