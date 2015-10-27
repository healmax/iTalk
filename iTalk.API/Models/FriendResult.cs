namespace iTalk.API.Models {
    /// <summary>
    /// 取得朋友的結果
    /// </summary>
    public class FriendResult : ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="friends">朋友名稱集合</param>
        public FriendResult(string[] friends) {
            this.Friend = friends;
        }

        /// <summary>
        /// 取得 朋友集合
        /// </summary>
        public string[] Friend { get; private set; }
    }
}