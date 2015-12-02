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
        /// <param name="relationshipId">關係 Id</param>
        /// <param name="senderId">發送者 Id</param>
        /// <param name="date">對話日期</param>
        public Chat(long relationshipId, long senderId, DateTime date)
            : base(date) {
            //if (string.IsNullOrEmpty(senderId)) {
            //    throw new ArgumentNullException("senderId");
            //}

            this.RelationshipId = relationshipId;
            this.SenderId = senderId;
        }

        /// <summary>
        /// 取得 Relationship Id
        /// </summary>
        [Required]
        [JsonIgnore]
        public long RelationshipId { get; private set; }

        /// <summary>
        /// 取得 Relationship
        /// </summary>
        [JsonIgnore]
        [ForeignKey("RelationshipId")]
        public virtual Relationship Relationship { get; private set; }

        /// <summary>
        /// 取得 發送者 Id
        /// </summary>
        [Required]
        public long SenderId { get; set; }

        /// <summary>
        /// 取得 發送者
        /// </summary>
        [JsonIgnore]
        [ForeignKey("SenderId")]
        //[InverseProperty("SendedChats")]
        public virtual iTalkUser Sender { get; private set; }
    }
}
