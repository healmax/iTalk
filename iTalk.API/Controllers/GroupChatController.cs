using iTalk.API.Models;
using iTalk.DAO;
using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 群組對話控制器
    /// </summary>
    [RoutePrefix("GroupChat")]
    public class GroupChatController : ChatControllerBase {
        /// <summary>
        /// 檢查群組
        /// </summary>
        /// <param name="groupId">群組Id</param>
        protected override async Task ValidateRelationship(long groupId) {
            await this.ValidateGroup(groupId);
        }

        /// <summary>
        /// 取得對話核心方法
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>對話集合</returns>
        protected override IQueryable<Chat> GetChats(long groupId) {
            return this.DbContext.Chats.Where(c => c.RelationshipId == groupId);
        }

        /// <summary>
        /// 傳送對話
        /// </summary>
        /// <param name="model">對話</param>
        /// <returns>傳送結果</returns>
        [Route("Dialog")]
        public async Task<ExecuteResult> Dialog(DialogViewModel model) {
            this.CheckModelState(model);
            await this.ValidateGroup(model.TargetId);
            Dialog dialog = new Dialog(model.TargetId, this.UserId, model.Date, model.Content);

            return await this.Chat(model.TargetId, dialog);
        }

        /// <summary>
        /// 傳送檔案(尚未實作)
        /// </summary>
        /// <param name="fileMessage">檔案訊息</param>
        /// <returns>執行結果</returns>
        [Route("File")]
        public Task<ExecuteResult> File(FileMessageViewModel fileMessage) {            
            throw new NotImplementedException();
        }

        /// <summary>
        /// 推送對話到相關的客戶端
        /// </summary>
        /// <param name="hub">SignalR HubContext</param>
        /// <param name="groupId">朋友或群組Id</param>
        /// <param name="model">對話</param>
        protected override async Task PushChatToClient(IHubContext hub, long groupId, Chat model) {
            var members = await this.DbContext.GroupMembers
                .Where(m => m.GroupId == model.RelationshipId)
                .Select(m => m.UserId.ToString())
                .ToArrayAsync();

            //var users = members
            //    .SkipWhile(id => id == this.UserId)
            //    .Select(id => id.ToString())
            //    .ToList();

            //model.TargetId = this.UserId;
            hub.Clients.Users(members).receiveGroupChat(groupId, model);
        }
    }
}
