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
    /// 群組控制器(測試中...)
    /// </summary>
    public class GroupController : DefaultApiController {
        /// <summary>
        /// 取得群組
        /// </summary>
        /// <returns>群組資訊</returns>
        public async Task<GroupResult> Get() {
            var groups = await this.DbContext.GroupMembers
                .Where(gm => gm.UserId == this.UserId)
                .Select(gm => new GroupResult.GroupDetail() {
                    Description = gm.Group.Description,
                    Id = gm.GroupId,
                    ImageUrl = gm.Group.ImageUrl,
                    Name = gm.Group.Name,
                    Thumbnail = gm.Group.Thumbnail
                })
                .ToArrayAsync();

            return new GroupResult(groups);
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

            // 檢查不存在或不是朋友的使用者
            var invalidUser = await model.Members.AsQueryable()
                .Except(this.DbContext.Friendships
                .Where(fs => fs.InviteeId == this.UserId && !model.Members.Contains(fs.InviteeId))
                .Select(u => u.Id))
                .ToArrayAsync();

            if (invalidUser.Length != 0) {
                throw this.CreateResponseException(HttpStatusCode.NotFound, "{0} {1} {2}", Resources.User, string.Join(",", invalidUser), Resources.NotExist);
            }

            Group group = new Group(model.Name, this.UserId, DateTime.Now) {
                Name = model.Name,
                Description = model.Description,
                // TODO : 圖片
            };

            try {
                this.DbContext.Groups.Add(group);
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
