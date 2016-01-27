using System;
using System.ComponentModel.DataAnnotations;

namespace iTalk.API.Models {
    /// <summary>
    /// 對話更新通知
    /// </summary>
    public class NoticeViewModel {
        /// <summary>
        /// 取得/設定 朋友或群組 Id
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// 取得/設定 最後讀取時間
        /// </summary>
        [Required]
        public DateTime ReadTime { get; set; }
    }
}