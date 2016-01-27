using iTalk.DAO;
using System;

namespace iTalk.API.Models {
    /// <summary>
    /// 回傳的使用者資訊
    /// </summary>
    public class FriendInfo : UserInfoBase {
        /// <summary>
        /// 取得/設定 我的最後讀取時間
        /// </summary>
        public DateTime MyReadTime { get; set; }

        /// <summary>
        /// 取得/設定 朋友最後讀取對話的時間
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 取得/設定 未讀取訊息數量
        /// </summary>
        public int UnreadMessageCount { get; set; }

        /// <summary>
        /// 取得/設定 最後一則對話
        /// </summary>
        public Chat LastChat { get; set; }

        ///// <summary>
        ///// 隱含轉換
        ///// </summary>
        ///// <param name="user">iTalkUser</param>
        ///// <returns>UserInfo</returns>
        //public static implicit operator FriendInfo(iTalkUser user) {
        //    return new FriendInfo {
        //        Alias = user.Alias,
        //        Id = user.Id,
        //        ImageUrl = user.PortraitUrl,
        //        PersonalSign = user.PersonalSign,
        //        Thumbnail = user.Thumb,
        //        UserName = user.UserName
        //    };
        //}
    }
}