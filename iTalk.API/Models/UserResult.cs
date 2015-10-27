using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace iTalk.API.Models {
    /// <summary>
    /// 使用者 View Model
    /// </summary>
    public class UserResult : ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <param name="isFriend">是否為朋友</param>
        public UserResult(string userName, bool isFriend) {
            this.UserName = userName;
            this.IsFriend = isFriend;
        }

        /// <summary>
        /// 取得/設定 使用者名稱
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 取得/設定 是否為朋友
        /// </summary>
        public bool IsFriend { get; private set; }
    }
}