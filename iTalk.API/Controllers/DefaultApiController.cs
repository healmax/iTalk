using ImageResizer;
using iTalk.API.Properties;
using iTalk.DAO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 預設控制器
    /// </summary>
    public class DefaultApiController : ApiController {
        /// <summary>
        /// 取得使用者 Id
        /// </summary>
        public long UserId {
            get { return this.User.Identity.GetUserId<long>(); }
        }

        /// <summary>
        /// 取得 iTalkDbContext
        /// </summary>
        protected internal iTalkDbContext DbContext {
            get { return this.Request.GetOwinContext().Get<iTalkDbContext>(); }
        }

        /// <summary>
        /// 取得 UserManager
        /// </summary>
        protected internal UserManager UserManager {
            get { return this.Request.GetOwinContext().GetUserManager<UserManager>(); }
        }

        /// <summary>
        /// 檢查 Model 是否有效
        /// </summary>
        /// <param name="model">要檢查的 Model</param>
        protected void CheckModelState(object model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideChatInfo);
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }
        }

        #region 圖片處理

        ///// <summary>
        ///// 圖片回調處理委派
        ///// </summary>
        ///// <param name="filename">檔案名稱</param>
        ///// <param name="content">圖片內容</param>
        ///// <param name="thumbnail">縮圖，base64 string 或網址</param>
        //protected delegate void ProcessImageCallback(string filename, byte[] content, string thumbnail);

        /// <summary>
        /// 處理上傳的圖片
        /// </summary>
        /// <param name="file">上傳的圖片</param>
        /// <param name="callback">回調處理</param>
        protected Portrait CreatePortrait(HttpPostedFile file) {
            string thumbnail = string.Empty;
            string ext = Path.GetExtension(file.FileName);
            string filename = string.Format("{0}{1}",
                Guid.NewGuid().ToString("N"),
                ext.Length > 4 ? ext.Substring(0, 4) : ext);

            using (Image image = Image.FromStream(file.InputStream)) {
                if (image.Width > 90 | image.Height > 90) {
                    using (MemoryStream ms = new MemoryStream()) {
                        file.InputStream.Seek(0, SeekOrigin.Begin);
                        ImageBuilder.Current.Build(image, ms, new ResizeSettings(90, 90, FitMode.Pad, null));
                        thumbnail = string.Format("data:{0};base64,{1}", MimeMapping.GetMimeMapping(file.FileName), Convert.ToBase64String(ms.ToArray()));
                    }
                }
            }

            byte[] content = new byte[file.InputStream.Length];
            file.InputStream.Seek(0, SeekOrigin.Begin);
            file.InputStream.Read(content, 0, content.Length);

            return new Portrait(filename, content, thumbnail, DateTime.UtcNow);
        }

        /// <summary>
        /// 檢查是否有上傳圖片
        /// </summary>
        /// <returns>上傳的圖片</returns>
        protected HttpPostedFile CheckPortrait() {
            HttpPostedFile file = HttpContext.Current.Request.Files["portrait"];

            if (file != null && file.ContentLength != 0) {
                string mime = MimeMapping.GetMimeMapping(file.FileName);

                if (!mime.StartsWith("image/")) {
                    throw this.CreateResponseException(HttpStatusCode.BadRequest, "只能上傳圖片");
                }
            }

            return file;
        }

        /// <summary>
        /// 取得 Chat Hub Context
        /// </summary>
        /// <returns>Chat Hub Context</returns>
        protected IHubContext HubContext {
            get { return GlobalHost.ConnectionManager.GetHubContext<ChatHub>(); }
        }

        #endregion

        ///// <summary>
        ///// 釋放資源
        ///// </summary>
        ///// <param name="disposing">是否釋放托管資源</param>
        //protected override void Dispose(bool disposing) {
        //    if (disposing && this._context.IsValueCreated) {
        //        this._context.Value.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}
    }
}