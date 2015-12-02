using iTalk.DAO;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

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