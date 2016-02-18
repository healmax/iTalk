using System;
using System.Net;
using System.Net.Http;
using System.Web;

namespace iTalk.API {
    /// <summary>
    /// 提供呼叫 iTalk Rest API 的基底位址與驗證
    /// </summary>
    public class iTalkClient : HttpClient {
        /// <summary>
        /// HttpClientHandler
        /// </summary>
        static HttpClientHandler _handler;

        /// <summary>
        /// Base Address
        /// </summary>
        static Uri baseAddress = new Uri(string.Format("http://{0}", HttpContext.Current.Request.Url.Authority));

        /// <summary>
        /// 初始化
        /// </summary>
        static iTalkClient() {
            _handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };
            var cookie = HttpContext.Current.Request.Cookies[".AspNet.Cookies"];

            if (cookie != null) {
                _handler.CookieContainer.Add(baseAddress, new Cookie(cookie.Name, cookie.Value));
            }
        }

        /// <summary>
        /// 建構函數
        /// </summary>
        public iTalkClient()
            : base(_handler) {
                this.BaseAddress = baseAddress;
        }
    }
}