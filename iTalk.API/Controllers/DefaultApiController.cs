using iTalk.API.Models;
using iTalk.DAO;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Linq;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 預設控制器
    /// </summary>
    public class DefaultApiController : ApiController {
        /// <summary>
        /// iTalkDbContext
        /// </summary>
        Lazy<iTalkDbContext> _context = new Lazy<iTalkDbContext>();

        /// <summary>
        /// 取得 iTalkDbContext
        /// </summary>
        protected iTalkDbContext DbContext {
            get { return this._context.Value; }
        }

        /// <summary>
        /// 取得 UserManager
        /// </summary>
        protected UserManager UserManager {
            get { return this.Request.GetOwinContext().GetUserManager<UserManager>(); }
        }

        ///// <summary>
        ///// 檢查使用者是否存在
        ///// </summary>
        ///// <param name="userName">使用者名稱</param>
        ///// <param name="throwException">是否拋出例外</param>
        ///// <returns>使用者</returns>
        //protected async Task<iTalkUser> ValidateUser(string userName, bool throwException) {
        //    iTalkUser user = await this.UserManager.FindByNameAsync(userName);

        //    if (user == null && throwException) {
        //        throw this.CreateResponseException(HttpStatusCode.NotFound, "使用者 {0} 不存在", userName);
        //    }

        //    return user;
        //}

        /// <summary>
        /// 檢查朋友關係
        /// </summary>
        /// <param name="friendName">朋友名稱</param>
        /// <param name="throwIfNotExist">指定之使用者不存在時是否拋出例外</param>
        /// <param name="throwIfNotFriend">指定之使用者不是朋友時是否拋出例外</param>
        /// <returns>朋友關聯狀態</returns>
        protected async Task<RelationshipStatus> ValidateRelationship(string friendName, bool throwIfNotExist = true, bool throwIfNotFriend = true) {
            iTalkUser invitee = await this.UserManager.FindByNameAsync(friendName);

            if (invitee == null) {
                if (throwIfNotExist) {
                    throw this.CreateResponseException(HttpStatusCode.NotFound, "使用者 {0} 不存在", friendName);
                }
                else {
                    return RelationshipStatus.NotExist;
                }
            }

            string id = this.User.Identity.GetUserId();
            //bool isFriend = await this.DbContext.Entry(invitee)
            //    .Collection(u => u.PassiveShips)
            //    .Query()
            //    .AnyAsync(rs => rs.UserId == id);
            bool isFriend = await this.DbContext.Relationships
                .AnyAsync(rs => rs.UserId == id && rs.InviteeId == invitee.Id);

            if (!isFriend) {
                if (throwIfNotFriend) {
                    throw this.CreateResponseException(HttpStatusCode.NotFound, "{0} 不是你的朋友", friendName);
                }
                else {
                    return RelationshipStatus.NotFriend;
                }
            }

            return RelationshipStatus.Friend;
        }

        /// <summary>
        /// 建立回應例外
        /// </summary>
        /// <param name="code">Http狀態碼</param>
        /// <param name="foramt">訊息內容</param>
        /// <param name="args">參數</param>
        /// <returns>回應例外</returns>
        protected HttpResponseException CreateResponseException(HttpStatusCode code, string format, params string[] args) {
            var response = this.Request.CreateResponse(code, new ExecuteResult(false, (int)code, string.Format(format, args)));
            return new HttpResponseException(response);
        }

        /// <summary>
        /// 取得 Model 上的錯誤訊息
        /// </summary>
        /// <returns></returns>
        protected string GetError() {

            StringBuilder errors = new StringBuilder();

            foreach (var pair in this.ModelState) {
                errors.AppendFormat("{0} : {1}", pair.Key, string.Join(",", pair.Value.Errors.Select(e => e.ErrorMessage)));
                errors.AppendLine();
            }

            return errors.ToString();
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">是否釋放托管資源</param>
        protected override void Dispose(bool disposing) {
            if (disposing && this._context.IsValueCreated) {
                this._context.Value.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}