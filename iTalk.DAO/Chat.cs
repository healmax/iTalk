using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 對話
    /// </summary>
    public class Chat {
        /// <summary>
        /// 建構函數 for EF
        /// </summary>
        protected Chat() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="senderId">發送者</param>
        /// <param name="receiverId">接收者</param>
        /// <param name="content">內容</param>
        /// <param name="date">對話日期</param>
        public Chat(string senderId, string receiverId, string content, DateTime date) {
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
            this.Content = content;
            this.Date = date;
        }

        /// <summary>
        /// 取得 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        /// <summary>
        /// 取得 發送者 Id
        /// </summary>
        [Required]
        public string SenderId { get; private set; }

        /// <summary>
        /// 取得 發送者
        /// </summary>
        [ForeignKey("SenderId")]
        [InverseProperty("SendedChats")]
        public virtual iTalkUser Sender { get; private set; }

        /// <summary>
        /// 取得 接收者 Id
        /// </summary>
        [Required]
        public string ReceiverId { get; private set; }

        /// <summary>
        /// 取得 接收者
        /// </summary>
        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceivedChats")]
        public iTalkUser Receiver { get; private set; }

        /// <summary>
        /// 取得 對話內容
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 0)]
        public string Content { get; private set; }

        /// <summary>
        /// 取得 對話時間
        /// </summary>
        [Required]
        public DateTime Date { get; private set; }
    }
}
