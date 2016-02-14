using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 好友控制器
    /// </summary>
    public class FriendController : DefaultApiController {
        /// <summary>
        /// 取得所有朋友
        /// </summary>
        /// <returns>所有朋友</returns>
        public async Task<ExecuteResult<FriendInfo[]>> Get() {
            var friends = await this.DbContext.Friendships
                .Where(rs => rs.UserId == this.UserId || rs.InviteeId == this.UserId)
                .Select(rs => new FriendInfo {
                    LastChat = rs.Chats.OrderByDescending(c => c.Date).FirstOrDefault(),
                    MyReadTime = rs.UserId == this.UserId ? rs.UserReadTime : rs.InviteeReadTime,
                    UnreadMessageCount = rs.Chats.Count(c => c.SenderId != this.UserId && c.Date > (rs.UserId == this.UserId ? rs.UserReadTime : rs.InviteeReadTime)),
                    ReadTime = rs.UserId == this.UserId ? rs.InviteeReadTime : rs.UserReadTime,
                    Alias = rs.InviteeId == this.UserId ? rs.User.Alias : rs.Invitee.Alias,
                    Id = rs.InviteeId == this.UserId ? rs.UserId : rs.InviteeId,
                    PortraitUrl = rs.InviteeId == this.UserId ? rs.User.Portrait.Filename : rs.Invitee.Portrait.Filename,
                    PersonalSign = rs.InviteeId == this.UserId ? rs.User.PersonalSign : rs.Invitee.PersonalSign,
                    Thumbnail = rs.InviteeId == this.UserId ? rs.User.Portrait.Thumbnail : rs.Invitee.Portrait.Thumbnail,
                    UserName = rs.InviteeId == this.UserId ? rs.User.UserName : rs.Invitee.UserName
                })
                .OrderBy(fi => fi.UserName)
                .ToArrayAsync();

            foreach (FriendInfo f in friends) {
                f.PortraitUrl = PortraitController.GenerateUrl(f.PortraitUrl);
            }

            return new ExecuteResult<FriendInfo[]>(friends);
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

            var result = await this.ValidateFriendship(model.Id, false, false);

            switch (result.Check) {
                case RelationCheck.HasRelation:
                    throw this.CreateResponseException(HttpStatusCode.Conflict, "{0} {1}", model.Id, Resources.AlreadyYourFriend);
                case RelationCheck.NoRelation:
                    // TODO : 目前先自動建立雙向關係
                    DateTime date = DateTime.UtcNow;
                    var fs = new Friendship(this.UserId, model.Id, RelationshipStatus.Accepted, date, RelationshipStatus.Accepted, date, date);
                    this.DbContext.Friendships.Add(fs);

                    try {
                        await this.DbContext.SaveChangesAsync();
                        await PushFriendshipToClient(fs);
                    }
                    catch (Exception ex) {
                        throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
                    }
                    break;
            }

            return new ExecuteResult(true);
        }

        /// <summary>
        /// 推送朋友關係
        /// </summary>
        /// <param name="fs">朋友關係</param>
        async Task PushFriendshipToClient(Friendship fs) {
            var fss = await this.DbContext.Users
                .Where(u => u.Id == fs.UserId || u.Id == fs.InviteeId)
                .Select(u => new FriendInfo {
                    Alias = u.Alias,
                    Id = u.Id,
                    MyReadTime = fs.UserId == u.Id ? fs.UserReadTime : fs.InviteeReadTime,
                    PersonalSign = u.PersonalSign,
                    PortraitUrl = u.Portrait.Filename,
                    ReadTime = fs.UserId == u.Id ? fs.InviteeReadTime : fs.UserReadTime,
                    Thumbnail = u.Portrait.Thumbnail,
                    UnreadMessageCount = 0,
                    UserName = u.UserName
                }).ToArrayAsync();

            foreach (var f in fss) {
                f.PortraitUrl = PortraitController.GenerateUrl(f.PortraitUrl);
                string id = f.Id == fs.UserId ? fs.InviteeId.ToString() : fs.UserId.ToString();
                this.HubContext.Clients.User(id).updateRelationship(f);
            }
        }
    }
}
