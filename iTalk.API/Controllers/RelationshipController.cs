using iTalk.API.Models;
using iTalk.DAO;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 關係控制器
    /// </summary>
    [Authorize]
    public class RelationshipController : DefaultApiController {
        /// <summary>
        /// 取得所有朋友
        /// </summary>
        /// <returns>所有朋友</returns>
        public async Task<HttpResponseMessage> Get() {
            string id = this.User.Identity.GetUserId();

            try {
                string[] friends = await this.DbContext.Relationships
                    .Where(rs => rs.UserId == id)
                    .OrderBy(rs => rs.UserId)
                    .Select(rs => rs.Invitee.UserName)
                    .ToArrayAsync();

                return this.Request.CreateResponse(new FriendResult(friends));
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="model">加好友 View Model</param>
        /// <returns>執行結果</returns>
        public async Task<ExecuteResult> Post(AddFriendViewModel model) {
            if (model == null || string.IsNullOrEmpty(model.FriendName)) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "需要提供欲加為好友的使用者的訊息");
            }

            if (model.FriendName.ToUpper() == this.User.Identity.Name.ToUpper()) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "不能加自己為好友喔");
            }

            RelationshipStatus status = await this.ValidateRelationship(model.FriendName, true, false);

            switch (status) {
                case RelationshipStatus.Friend:
                    throw this.CreateResponseException(HttpStatusCode.Conflict, "{0} 已經是你的朋友了", model.FriendName);
                case RelationshipStatus.NotFriend:
                    string inviteeId = (await this.UserManager.FindByNameAsync(model.FriendName)).Id;
                    Relationship ship = new Relationship(this.User.Identity.GetUserId(), inviteeId);
                    this.DbContext.Relationships.Add(ship);

                    try {
                        await this.DbContext.SaveChangesAsync();
                    }
                    catch (Exception ex) {
                        throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
                    }
                    break;
            }

            return new ExecuteResult(true);
        }
    }
}
