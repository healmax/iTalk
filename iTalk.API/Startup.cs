﻿using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(iTalk.API.Startup))]

namespace iTalk.API {
    /// <summary>
    /// 啟動程序
    /// </summary>
    public partial class Startup {
        /// <summary>
        /// 設置...
        /// </summary>
        /// <param name="app">IAppBuilder</param>
        public void Configuration(IAppBuilder app) {
            app.Use<Providers.GlobalExceptionMiddleware>();
            this.ConfigureAuth(app);

            // SignalR 序列化設定...
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            // 使用 User Id 來存取 SignalR Hub
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());

            app.MapSignalR();
        }
    }
}