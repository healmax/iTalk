using iTalk.API.Models;
using iTalk.API.Properties;
using iTalk.DAO;
using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 對話控制器
    /// </summary>
    public abstract class ChatControllerBase : DefaultApiController {
        /// <summary>
        /// 取得對話
        /// </summary>
        /// <param name="targetId">使用者或群組 Id</param>
        /// <param name="top">篩選前 N 筆資料</param>
        /// <param name="skip">跳過前 N 筆資料</param>
        /// <param name="before">篩選某個時間點前的資料</param>
        /// <param name="after">篩選某個時間點後的資料</param>
        /// <returns>對話</returns>
        public async Task<ChatResult> Get(long targetId, int? top = null, int? skip = null, DateTime? after = null, DateTime? before = null) {
            await this.ValidateRelationship(targetId);

            var query = this.GetChats(targetId);

            if (top != null) {
                query.Take(top.Value);
            }

            if (skip != null) {
                query.Skip(skip.Value);
            }

            if (after != null) {
                query = query.Where(c => c.Date >= after);
            }

            if (before != null) {
                query = query.Where(c => c.Date <= before);
            }

            return new ChatResult(await query.ToArrayAsync());
        }

        /// <summary>
        /// 檢查關係
        /// </summary>
        /// <param name="targetId">關係 Id</param>
        protected abstract Task ValidateRelationship(long targetId);

        /// <summary>
        /// 取得對話
        /// </summary>
        /// <param name="targetId">使用者或群組 Id</param>
        /// <returns>對話集合</returns>
        protected abstract IQueryable<Chat> GetChats(long targetId);

        /// <summary>
        /// 傳送對話
        /// </summary>
        /// <param name="model">對話</param>
        /// <param name="chat">對話</param>
        /// <returns>執行結果</returns>
        protected async Task<ExecuteResult> Chat(ChatViewModel model, Chat chat) {
            this.DbContext.Chats.Add(chat);

            try {
                await this.DbContext.SaveChangesAsync();

                // 暫放 Hub
                var hub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                await this.PushChatToClient(hub, model);
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult(true);
        }

        /// <summary>
        /// 檢查 Model 是否有效
        /// </summary>
        /// <param name="model">要檢查的 Model</param>
        protected void CheckModelState(ChatViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, Resources.NotProvideChatInfo);
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }
        }

        /// <summary>
        /// 推送對話到相關的客戶端
        /// </summary>
        /// <param name="hub">SignalR HubContext</param>
        /// <param name="model">對話</param>
        protected abstract Task PushChatToClient(IHubContext hub, ChatViewModel model);
    }
}
