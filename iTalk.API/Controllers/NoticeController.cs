using iTalk.API.Models;
using iTalk.DAO;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 對話更新通知控制器
    /// </summary>
    public class NoticeController : DefaultApiController {
        /// <summary>
        /// 更新讀取對話時間
        /// </summary>
        /// <param name="targetId">朋友或群組 Id</param>
        /// <param name="readTime">最後讀取時間</param>
        /// <returns></returns>
        public async Task<ExecuteResult> Post(NoticeViewModel model) {
            this.CheckModelState(model);

            if (model.Id > 0) {
                // 更新與朋友的對話通知
                var result = await this.ValidateFriendship(model.Id);

                if (result.Target[this.UserId] < model.ReadTime) {
                    result.Target[this.UserId] = model.ReadTime;
                    await this.DbContext.SaveChangesAsync();
                    this.PushNoticeToFriend(model.ReadTime, model.Id.ToString());
                }
            }
            else if (model.Id < 0) {
                // 更新群組的對話通知
                await this.ValidateGroup(model.Id);
                GroupMember[] members = await this.DbContext.GroupMembers
                    .Where(m => m.GroupId == model.Id)
                    .ToArrayAsync();
                GroupMember me = members.First(m => m.UserId == this.UserId);

                if (me.ReadTime < model.ReadTime) {
                    me.ReadTime = model.ReadTime;
                    await this.DbContext.SaveChangesAsync();

                    var otherMemberIds = members
                        .SkipWhile(m => m.UserId == this.UserId)
                        .Select(m => m.UserId.ToString())
                        .ToArray();
                    this.PushNoticeToGroupMembers(model.ReadTime, model.Id, otherMemberIds);
                }
            }
            else {
                throw this.CreateResponseException(HttpStatusCode.NotAcceptable, "錯誤的朋友或群組 Id");
            }

            return new ExecuteResult();
        }

        /// <summary>
        /// 推送對話更新到朋友的客戶端
        /// </summary>
        /// <param name="readTime">我的最後讀取時間</param>
        /// <param name="friendId">朋友 Id</param>
        void PushNoticeToFriend(DateTime readTime, string friendId) {
            this.HubContext.Clients.User(friendId).updateFriendReadTime(this.UserId, readTime);
        }

        /// <summary>
        /// 推送對話更新到群組所有成員的客戶端
        /// </summary>
        /// <param name="readTime">我的最後讀取時間</param>
        /// <param name="memberIds">群組成員 Id 集合</param>
        void PushNoticeToGroupMembers(DateTime readTime, long groupId, params string[] memberIds) {
            this.HubContext.Clients.Users(memberIds).updateGroupMemberReadTime(groupId, this.UserId, readTime);
        }
    }
}
