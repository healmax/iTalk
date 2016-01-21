using iTalk.API.Areas.Admin.Models;
using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 好友控制器
    /// </summary>
    public class FriendController : DefaultApiController {
        /// <summary>
        /// 取得所有朋友
        /// </summary>
        /// <returns>所有朋友</returns>
        public async Task<ExecuteResult<FriendResult[]>> Get() {
            try {
                var friends = await this.DbContext.Friendships
                    .Where(rs => rs.InviteeId == this.UserId)
                    .Select(rs => new FriendResult {
                        Alias = rs.User.Alias,
                        Id = rs.User.Id,
                        ImageUrl = rs.User.PortraitUrl,
                        PersonalSign = rs.User.PersonalSign,
                        Thumbnail = rs.User.Thumb,
                        UserName = rs.User.UserName,
                        ReadTime = rs.ReadTime
                    }).ToArrayAsync();

                return new ExecuteResult<FriendResult[]>(friends);
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
            if (model == null || !this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NeedProvideUserNameToAdd);
            }

            if (model.Id == this.UserId) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.CannotAddSelfAsFriend);
            }

            RelationCheck status = await this.ValidateFriendship(model.Id, false, false);

            switch (status) {
                case RelationCheck.In:
                    throw this.CreateResponseException(HttpStatusCode.Conflict, "{0} {1}", model.Id, Resources.AlreadyYourFriend);
                case RelationCheck.Not:
                    // TODO : 目前先自動建立雙向關係
                    DateTime date = DateTime.UtcNow;
                    this.DbContext.Friendships.Add(new Friendship(this.UserId, model.Id, RelationshipStatus.Pending, date, date));
                    this.DbContext.Friendships.Add(new Friendship(model.Id, this.UserId, RelationshipStatus.Pending, date, date));

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
