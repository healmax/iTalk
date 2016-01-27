using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTalk.DAO {
    /// <summary>
    /// 對話
    /// </summary>
    public class Dialog : Chat {
        /// <summary>
        /// 建構函數 for EF
        /// </summary>
        protected Dialog() { }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="senderId">發送者 Id</param>
        /// <param name="relationId">朋友關係或群組 Id</param>
        /// <param name="date">對話日期</param>
        /// <param name="content">內容</param>
        public Dialog(long senderId, long relationId, DateTime date, string content)
            : base(senderId, relationId, date) {
            if (string.IsNullOrEmpty(content)) {
                throw new ArgumentNullException("content");
            }

            this.Content = content;
        }

        /// <summary>
        /// 取得 對話內容
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Content { get; private set; }
    }
}
