using iTalk.API.Models;
using iTalk.API.Properties;
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
        public async Task<ExecuteResult<UserResult>> Get(string userName) {
            if (string.IsNullOrEmpty(userName)) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideUserName);
            }

            iTalkUser target = await this.UserManager.FindByNameAsync(userName);

            if (target == null) {
                throw this.CreateResponseException(HttpStatusCode.NotFound, Resources.UserNotExist);
            }

            bool isFriend = await this.DbContext.Friendships
                .AnyAsync(rs => rs.UserId == this.UserId && rs.InviteeId == target.Id);

            return new ExecuteResult<UserResult>(new UserResult(target.Id, target.UserName, target.Alias, target.PersonalSign, isFriend));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>更新結果</returns>
        public async Task<ExecuteResult> Put(UserViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideUserInfo);
            }

            iTalkUser user = await this.UserManager.FindByNameAsync(this.User.Identity.Name);
            user.PersonalSign = model.PersonalSign;

            // TODO : 更新個人圖片
            try {
                this.DbContext.SaveChanges();
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult(true);
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model">帳戶 View Model</param>
        /// <returns>註冊結果</returns>
        [AllowAnonymous]
        [DefaultTestFriendFilter]
        public async Task<ExecuteResult> Post(AccountViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NeedUserNameAndPassword);
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }

            iTalkUser user = new iTalkUser() {
                UserName = model.UserName,
                Alias = model.Alias,
                PersonalSign = model.PersonalSign
            };
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
