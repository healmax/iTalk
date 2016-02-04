using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 通用字串控制器
    /// </summary>
    [AllowAnonymous]
    public class ConstantsController : ApiController {
        /// <summary>
        /// 動態生成 javascript 內容，提供可用的常數
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get() {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            string json = "var constants = " + JsonConvert.SerializeObject(new Constants());
            message.Content = new StringContent(json);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/javascript");

            return message;
        }
    }
}
