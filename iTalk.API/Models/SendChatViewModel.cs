using System;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Models {
    /// <summary>
    /// 傳送對話 View Model
    /// </summary>
    public class SendChatViewModel {
        /// <summary>
        /// 取得/設定 朋友名稱
        /// </summary>
        [MinLength(1, ErrorMessage = "沒有指定對話的朋友名稱")]
        public string FriendName { get; set; }

        /// <summary>
        /// 取得/設定 對話內容
        /// </summary>
        [StringLength(255, MinimumLength = 1, ErrorMessage = "沒有對話內容")]
        public string Content { get; set; }

        /// <summary>
        /// 取得/設定 對話時間
        /// </summary>
        public DateTime Date { get; set; }
    }
}