﻿using iTalk.API.Models;
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
        public async Task<ExecuteResult<Chat[]>> Get(long targetId, int? top = null, int? skip = null, DateTime? after = null, DateTime? before = null) {
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

            return new ExecuteResult<Chat[]>(await query.ToArrayAsync());
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
        /// <param name="chat">對話</param>
        /// <param name="targetId">朋友或群組 Id</param>
        /// <returns>執行結果</returns>
        protected async Task<ExecuteResult> Chat(long targetId, Chat chat) {
            this.DbContext.Chats.Add(chat);

            try {
                await this.DbContext.SaveChangesAsync();

                await this.PushChatToClient(this.HubContext, targetId, chat);
            }
            catch (Exception ex) {
                throw this.CreateResponseException(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new ExecuteResult();
        }

        /// <summary>
        /// 推送對話到相關的客戶端
        /// </summary>
        /// <param name="hub">SignalR HubContext</param>
        /// <param name="targetId">朋友或群組 Id</param>
        /// <param name="model">對話</param>
        protected abstract Task PushChatToClient(IHubContext hub, long targetId, Chat model);
    }
}
