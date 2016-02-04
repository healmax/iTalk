using iTalk.DAO;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 圖片控制器
    /// </summary>
    public class PortraitController : DefaultApiController {
        //public const string CONTROLLER_NAME = "portrait";
        
        /// <summary>
        /// 取得圖片
        /// </summary>
        /// <param name="filename">檔名</param>
        /// <returns>圖片</returns>
        public async Task<HttpResponseMessage> Get(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                throw this.CreateResponseException(HttpStatusCode.BadRequest, string.Empty);
            }

            Portrait portrait = await this.DbContext.Portraits
                .FirstOrDefaultAsync(p => p.Filename.ToUpper() == filename.ToUpper());

            if (portrait == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StreamContent(new MemoryStream(portrait.Content));
            message.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(portrait.Filename));

            return message;
        }

        /// <summary>
        /// 產生圖片網址
        /// </summary>
        /// <param name="filename">檔案名稱</param>
        /// <returns>圖片網址</returns>
        [NonAction]
        public static string GenerateUrl(string filename) {
            return string.IsNullOrEmpty(filename) ? string.Empty : string.Format("/portrait?filename={0}", filename);
        }
    }
}
