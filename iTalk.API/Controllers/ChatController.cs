using iTalk.API.Models;
using iTalk.DAO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace iTalk.API.Controllers {
    /// <summary>
    /// 對話控制器
    /// </summary>
    [Authorize]
    public class ChatController : DefaultApiController {
        /// <summary>
        /// 取得對話
        /// </summary>
        /// <param name="userName">朋友名稱</param>
        /// <param name="startTime">對話開始時間(可選)</param>
        /// <returns>對話</returns>
        public async Task<ChatResult> Get(string userName, DateTime? startTime = null) {
            if (string.IsNullOrEmpty(userName)) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "未指定朋友名稱");
            }

            if (userName.ToUpper() == this.User.Identity.Name.ToUpper()) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "不能自言自語喔");
            }

            await this.ValidateRelationship(userName);
            string userId = this.User.Identity.GetUserId();

            try {
                IEnumerable<Chat> chats = await this.DbContext.Chats
                    .Where(c => (c.SenderId == userId && c.Receiver.UserName == userName) ||
                        (c.Sender.UserName == userName && c.ReceiverId == userId))
                        .Where(c => startTime.HasValue ? c.Date > startTime.Value : true)
                    .OrderBy(c => c.Date)
                    .ToArrayAsync();

                return new ChatResult(chats);
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 傳送對話
        /// </summary>
        /// <param name="friendName">朋友名稱</param>
        /// <returns>傳送結果</returns>
        public async Task<ExecuteResult> Post(SendChatViewModel model) {
            if (model == null) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, "沒有提供對話的必要資訊");
            }

            if (!this.ModelState.IsValid) {
                throw this.CreateResponseException(HttpStatusCode.Forbidden, this.GetError());
            }

            await this.ValidateRelationship(model.FriendName);

            string friendId = (await this.UserManager.FindByNameAsync(model.FriendName)).Id;
            Chat chat = new Chat(this.User.Identity.GetUserId(), friendId, model.Content, model.Date);
            this.DbContext.Chats.Add(chat);

            try {
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult(true);
        }
    }
}
