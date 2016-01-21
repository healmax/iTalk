using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTalk.API.Areas.Admin.Models {
    /// <summary>
    /// 好友通知
    /// </summary>
    public class FriendNoticeResult {
        /// <summary>
        /// 取得/設定 朋友 Id
        /// </summary>
        public long FriendId { get; set; }

        /// <summary>
        /// 取得/設定 朋友最後讀取時間
        /// </summary>
        public DateTime FriendReadTime { get; set; }

        /// <summary>
        /// 取得/設定 我的最後讀取時間
        /// </summary>
        public DateTime MyReadTime { get; set; }
    }
}