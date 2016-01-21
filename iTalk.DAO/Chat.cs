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
        /// <param name="receiverId">接收者 Id</param>
        /// <param name="date">對話日期</param>
        public Chat(long senderId, long receiverId, DateTime date)
            : base(date) {
            //if (string.IsNullOrEmpty(senderId)) {
            //    throw new ArgumentNullException("senderId");
            //}

            //this.RelationshipId = relationshipId;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }

        ///// <summary>
        ///// 取得 Relationship Id
        ///// </summary>
        //[Required]
        //public long RelationshipId { get; private set; }

        ///// <summary>
        ///// 取得 Relationship
        ///// </summary>
        //[JsonIgnore]
        //[ForeignKey("RelationshipId")]
        //public virtual Relationship Relationship { get; private set; }

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
        //[InverseProperty("SendedChats")]
        public virtual iTalkUser Sender { get; private set; }

        /// <summary>
        /// 取得 接收者 Id
        /// </summary>
        [Required]
        public long ReceiverId { get; private set; }

        /// <summary>
        /// 取得 接收者
        /// 可能是使用或群組
        /// </summary>
        [JsonIgnore]
        [ForeignKey("ReceiverId")]
        public virtual ITarget Receiver { get; private set; }
    }
}
