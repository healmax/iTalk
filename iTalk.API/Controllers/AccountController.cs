using iTalk.API.Models;
using iTalk.DAO;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 帳號控制器
    /// </summary>
    public class AccountController : DefaultApiController {
        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <returns>使用者資訊</returns>
        [Authorize]
        public async Task<UserResult> Get(string userName) {
            if (string.IsNullOrEmpty(userName)) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "未指定使用者名稱");
            }

            iTalkUser user = await this.UserManager.FindByNameAsync(userName);

            if (user == null) {
                throw this.CreateResponseException(HttpStatusCode.NotFound, "查無此人");
            }

            string myId = this.User.Identity.GetUserId();
            bool isFriend = await this.DbContext.Relationships
                .AnyAsync(rs => rs.UserId == myId && rs.InviteeId == user.Id);

            return new UserResult(user.UserName, isFriend);
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model">帳戶 View Model</param>
        /// <returns>註冊結果</returns>
        public async Task<ExecuteResult> Register(AccountViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "需要使用者名稱與密碼");
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }

            iTalkUser user = new iTalkUser() { UserName = model.UserName };
            IdentityResult result;

            try {
                result = await this.UserManager.CreateAsync(user, model.Password);
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.BadRequest, ex.Message);
            }

            if (!result.Succeeded) {
                throw this.CreateResponseException(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
            }

            return new ExecuteResult(true);
        }
    }
}
