using System.Collections.Generic;
namespace iTalk.API.Models {
    /// <summary>
    /// 取得朋友的結果
    /// </summary>
    public class FriendResult : ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="friends">朋友名稱集合</param>
        public FriendResult(IEnumerable<string> friends) {
            this.Friends = friends;
        }

        /// <summary>
        /// 取得 朋友集合
        /// </summary>
        public IEnumerable<string> Friends { get; private set; }
    }
}