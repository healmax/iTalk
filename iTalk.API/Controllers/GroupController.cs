using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 群組控制器
    /// </summary>
    public class GroupController : DefaultApiController {
        /// <summary>
        /// 取得群組
        /// </summary>
        /// <returns>群組資訊</returns>
        public async Task<ExecuteResult<GroupInfo[]>> Get() {
            var groups = await this.DbContext.GroupMembers
                .Where(m => m.UserId == this.UserId)
                .Select(m => m.Group)
                .Select(g => new GroupInfo {
                    Description = g.Description,
                    Id = g.Id,
                    PortraitUrl = g.Portrait.Filename,
                    Name = g.Name,
                    Thumbnail = g.Portrait.Thumbnail,
                    LastChat = g.Chats.OrderByDescending(c => c.Date).FirstOrDefault(),
                    UnreadMessageCount = g.Chats.Count(c => c.SenderId != this.UserId && c.Date > g.Members.FirstOrDefault(m => m.UserId == this.UserId).ReadTime),
                    Members = g.Members.Select(gm => new GroupInfo.GroupMember {
                        Id = gm.UserId,
                        ReadTime = gm.ReadTime
                    })
                })
                .OrderBy(g => g.Name)
                .ToArrayAsync();

            foreach (GroupInfo g in groups) {
                g.PortraitUrl = PortraitController.GenerateUrl(g.PortraitUrl);
            }

            return new ExecuteResult<GroupInfo[]>(groups);
        }

        /// <summary>
        /// 取得群組成員
        /// </summary>
        /// <param name="groupId">群組Id</param>
        /// <returns>群組成員</returns>
        public async Task<ExecuteResult<UserInfoBase[]>> Get(long groupId) {
            await this.ValidateGroup(groupId);

            var members = await this.DbContext.GroupMembers
                .Where(gm => gm.GroupId == groupId)
                .Select(gm => new UserInfoBase {
                    Alias = gm.User.Alias,
                    Id = gm.UserId,
                    PortraitUrl = gm.User.Portrait.Filename,
                    PersonalSign = gm.User.PersonalSign,
                    Thumbnail = gm.User.Portrait.Thumbnail,
                    UserName = gm.User.UserName
                })
                .ToArrayAsync();

            foreach (var user in members) {
                user.PortraitUrl = PortraitController.GenerateUrl(user.PortraitUrl);
            }

            return new ExecuteResult<UserInfoBase[]>(members);
        }

        /// <summary>
        /// 建立群組
        /// </summary>
        /// <parameter name="name">群組名稱 (string)</parameter>
        /// <parameter name="members">群組成員 Id 集合 (long[])</parameter>
        /// <parameter name="portrait">個人圖片，非必要 (file)</parameter>
        /// <returns>執行結果</returns>
        public async Task<ExecuteResult> Post(/*, HttpPostedFileBase image = null*/) {
            #region 參數檢查
            //this.CheckModelState(model);

            string name = HttpContext.Current.Request.Form["name"];

            if (string.IsNullOrEmpty(name)) {
                throw this.CreateResponseException(HttpStatusCode.BadRequest, "未指定群組名稱");
            }

            List<long> memberIds = new List<long>();
            string raw = HttpContext.Current.Request.Form["members"];

            if (!string.IsNullOrEmpty(raw)) {
                try {
                    memberIds = raw
                        .Split(',')
                        .Select(str => long.Parse(str.Trim()))
                        .Distinct()
                        .SkipWhile(id => id == this.UserId)
                        .ToList();
                }
                catch (Exception) {
                    throw this.CreateResponseException(HttpStatusCode.BadRequest, "不合法的群組成員 Id");
                }
            }

            HttpPostedFile file = CheckPortrait();

            #endregion

            // 檢查不存在或不是朋友的使用者
            long[] invalidUsers = null;
            await Task.Run(() => {
                invalidUsers = memberIds
                .Except(this.DbContext.Friendships
                    .Where(fs => fs.UserId == this.UserId || fs.InviteeId == this.UserId)
                    .Select(u => u.UserId == this.UserId ? u.InviteeId : u.UserId))
                .ToArray();

                if (invalidUsers.Length != 0) {
                    throw this.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.User, string.Join(",", invalidUsers), Resources.NotExist);
                }

                memberIds.Add(this.UserId);
            });

            DateTime createTime = DateTime.UtcNow;
            Group group = this.DbContext.Groups.Add(new Group(name, this.UserId, createTime));

            if (file != null) {
                try {
                    Portrait portrait = this.CreatePortrait(file);
                    this.DbContext.Portraits.Add(portrait);
                    group.Portrait = portrait;
                }
                catch (Exception) {
                    throw this.CreateResponseException(HttpStatusCode.BadRequest, "只能上傳圖片");
                }
            }
            
            foreach (long id in memberIds) {
                this.DbContext.GroupMembers.Add(new GroupMember(id, group, RelationshipStatus.Pending, createTime, createTime));
            }

            try {
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult();
        }

        ///// <summary>
        ///// 更新群組
        ///// </summary>
        ///// <returns></returns>
        //public async Task<ExecuteResult> Put(long id, Delta<Group> model) {
        //    if (model == null) {
        //        throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideNessaryInfo);
        //    }

        //    if (!this.ModelState.IsValid) {
        //        throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
        //    }

        //    Group group = await this.FindGroup(id);

        //    // TODO : 圖片

        //    try {
        //        await this.DbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex) {
        //        this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //    return new ExecuteResult();
        //}

        /// <summary>
        /// 刪除群組
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<ExecuteResult> Delete(long groupId) {
            Group group = await this.FindGroup(groupId);

            if (group.CreatorId != this.UserId) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.YouDoNotHavePermissionDeleteGroup);
            }

            try {
                this.DbContext.Groups.Remove(group);
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult();
        }

        /// <summary>
        /// 找尋群組
        /// </summary>
        /// <param name="groupId">群組 Id</param>
        /// <returns>群組</returns>
        async Task<Group> FindGroup(long groupId) {
            Group group = await this.DbContext.Groups.FindAsync(groupId);

            if (group == null) {
                throw this.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}",
                    Resources.Group, groupId, Resources.NotExist);
            }

            return group;
        }
    }
}
