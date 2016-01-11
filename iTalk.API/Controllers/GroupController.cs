﻿using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 群組控制器(測試中...)
    /// </summary>
    public class GroupController : DefaultApiController {
        /// <summary>
        /// 取得群組
        /// </summary>
        /// <returns>群組資訊</returns>
        public async Task<ExecuteResult<GroupResult[]>> Get() {
            var groups = await this.DbContext.Groups
                .Where(g => g.Members.Any(gm => gm.UserId == this.UserId))
                .Select(g => new GroupResult() {
                    Description = g.Description,
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    Name = g.Name,
                    Thumbnail = g.Thumbnail,
                    Members = g.Members.Select(m => m.UserId)
                })
                .ToArrayAsync();

            return new ExecuteResult<GroupResult[]>(groups);
        }

        /// <summary>
        /// 取得群組
        /// </summary>
        /// <param name="groupId">群組Id</param>
        /// <returns>群組</returns>
        public async Task<ExecuteResult<UserResult[]>> Get(long groupId) {
            await this.ValidateGroup(groupId);
            var members = await this.DbContext.GroupMembers
                .Where(gm => gm.GroupId == groupId)
                .Select(gm => new UserResult() {
                    Id = gm.UserId,
                    UserName = gm.User.UserName,
                    Alias = gm.User.Alias,
                    PersonalSign = gm.User.PersonalSign,
                    IsFriend = this.DbContext.Friendships.Any(fs => fs.UserId == this.UserId && fs.InviteeId == gm.UserId)
                })
                .ToArrayAsync();

            return new ExecuteResult<UserResult[]>(members);
        }

        /// <summary>
        /// 建立群組
        /// </summary>
        /// <returns>執行結果</returns>
        public async Task<ExecuteResult> Post(CreateGroupViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideNessaryInfo);
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }

            var memberIds = model.Members
                .Distinct()
                .SkipWhile(id => id == this.UserId)
                .ToList();

            // 檢查不存在或不是朋友的使用者
            long[] invalidUsers = null;
            await Task.Run(() => invalidUsers = memberIds
                .Except(this.DbContext.Friendships
                    .Where(fs => fs.UserId == this.UserId /*&& !model.Members.Contains(fs.InviteeId)*/)
                    .Select(u => u.InviteeId))
                .ToArray());

            if (invalidUsers.Length != 0) {
                throw this.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.User, string.Join(",", invalidUsers), Resources.NotExist);
            }

            DateTime createTime = DateTime.Now;
            Group group = this.DbContext.Groups.Add(new Group(model.Name, this.UserId, createTime) {
                Description = model.Description,
                // TODO : 圖片
            });

            memberIds.Add(this.UserId);

            foreach (long id in memberIds) {
                this.DbContext.GroupMembers.Add(new GroupMember(id, group, RelationshipStatus.Pending, createTime));
            }

            try {
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult();
        }

        /// <summary>
        /// 更新群組
        /// </summary>
        /// <returns></returns>
        public async Task<ExecuteResult> Put(GroupViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideNessaryInfo);
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }

            Group group = await this.FindGroup(model.Id);
            group.Name = model.Name;
            group.Description = model.Description;
            // TODO : 圖片

            try {
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult();
        }

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
