using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace iTalk.API.Models {
    /// <summary>
    /// 對話 View Model
    /// </summary>
    public abstract class ChatViewModel {
        /// <summary>
        /// 取得/設定 朋友或群組 Id
        /// </summary>
        [Required]
        public long TargetId { get; set; }

        /// <summary>
        /// 取得/設定 對話時間
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// 傳送對話 View Model
    /// </summary>
    public class DialogViewModel : ChatViewModel {
        /// <summary>
        /// 取得/設定 對話內容
        /// </summary>
        [StringLength(255, MinimumLength = 1, ErrorMessage = "沒有對話內容")]
        public string Content { get; set; }
    }

    /// <summary>
    /// 傳送檔案 View Model
    /// </summary>
    public class FileMessageViewModel : ChatViewModel {
        /// <summary>
        /// 取得/設定 檔案名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 取得/設定 檔案內容
        /// </summary>
        public Stream Content { get; set; }
    }
}