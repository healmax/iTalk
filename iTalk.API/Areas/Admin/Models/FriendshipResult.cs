using iTalk.API.Models;
using iTalk.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTalk.API.Areas.Admin.Models {
    /// <summary>
    /// 朋友關係
    /// </summary>
    public class FriendResult : UserInfo {
        /// <summary>
        /// 取得/設定 朋友最後讀取對話的時間
        /// </summary>
        public DateTime ReadTime { get; set; }

        ///// <summary>
        ///// 取得/設定 未讀取訊息數量
        ///// </summary>
        //public int NotReadMessageCount { get; set; }

        ///// <summary>
        ///// 取得/設定 最後一則對話
        ///// </summary>
        //public Chat LastChat { get; set; }
    }
}