using iTalk.API.Models;
using iTalk.DAO;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 對話控制器
    /// </summary>
    [RoutePrefix("Chat")]
    public class ChatController : ChatControllerBase {
        /// <summary>
        /// 檢查朋友關係
        /// </summary>
        /// <param name="friendId">朋友 Id</param>
        protected override async Task ValidateRelationship(long friendId) {
            await this.ValidateFriendship(friendId);
        }

        /// <summary>
        /// 取得對話
        /// </summary>
        /// <param name="friendId">朋友 Id</param>
        /// <returns>對話集合</returns>
        protected override IQueryable<Chat> GetChats(long friendId) {
            return this.DbContext.Friendships
                .Where(fs => (fs.UserId == this.UserId && fs.InviteeId == friendId) ||
                    (fs.UserId == friendId && fs.InviteeId == this.UserId))
                .SelectMany(fs => fs.Chats);
        }

        /// <summary>
        /// 傳送對話
        /// </summary>
        /// <param name="model">對話</param>
        /// <returns>傳送結果</returns>
        [Route("Dialog")]
        public async Task<ExecuteResult> Dialog(DialogViewModel model) {
            this.CheckModelState(model);

            // 檢查有無此人、是否為好友
            long friendId = model.TargetId;
            var result = await this.ValidateFriendship(friendId);
            Dialog dialog = new Dialog(this.UserId, result.Target.Id, model.Date, model.Content);

            return await this.Chat(friendId, dialog);
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
        /// 推送對話到自己與特定朋友的客戶端
        /// </summary>
        /// <param name="hub">SignalR Hub</param>
        /// <param name="friendId">朋友 Id</param>
        /// <param name="model">對話</param>
        protected override async Task PushChatToClient(IHubContext hub, long friendId, Chat model) {
            hub.Clients.User(this.UserId.ToString()).receiveChat(friendId, model);
            hub.Clients.User(friendId.ToString()).receiveChat(this.UserId, model);

            await Task.FromResult<object>(null);
        }
    }
}
