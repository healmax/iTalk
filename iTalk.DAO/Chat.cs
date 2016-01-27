using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 聊天抽象基類
    /// </summary>
    public abstract class Chat : EntityBase {
        /// <summary>
        /// 建構函數 for EF
        /// </summary>
        protected Chat() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="senderId">發送者 Id</param>
        /// <param name="relationId">朋友關係或群組 Id</param>
        /// <param name="date">對話日期</param>
        public Chat(long senderId, long relationId, DateTime date)
            : base(date) {
            this.SenderId = senderId;
            this.RelationId = relationId;
        }

        /// <summary>
        /// 取得 發送者 Id
        /// </summary>
        [Required]
        public long SenderId { get; private set; }

        /// <summary>
        /// 取得 發送者
        /// </summary>
        [JsonIgnore]
        [ForeignKey("SenderId")]
        public virtual iTalkUser Sender { get; private set; }

        /// <summary>
        /// 取得 朋友或群組關係 Id
        /// </summary>
        public long RelationId { get; private set; }

        /// <summary>
        /// 取得 朋友關係或群組
        /// </summary>
        [JsonIgnore]
        [ForeignKey("RelationId")]
        public virtual Relationship Relation { get; private set; }
    }
}
