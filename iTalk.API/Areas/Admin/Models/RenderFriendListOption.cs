namespace iTalk.API.Areas.Admin.Models {
    /// <summary>
    /// Render 好友列表的選項
    /// </summary>
    public class RenderFriendListOption {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="showCheckbox">是否顯示 Checkbox</param>
        /// <param name="showFriendCount">是否顯示好友數量</param>
        /// <param name="showLastChat">是否顯示最後一筆的對話</param>
        public RenderFriendListOption(bool showCheckbox = true, bool showFriendCount = true, bool showLastChat = true) {
            this.ShowCheckbox = showCheckbox;
            this.ShowFriendCount = ShowFriendCount;
            this.ShowLastChat = this.ShowLastChat;
        }

        /// <summary>
        /// 取得/設定 是否顯示 Checkbox
        /// </summary>
        public bool ShowCheckbox { get; set; }

        /// <summary>
        /// 取得/設定 是否顯示好友數量
        /// </summary>
        public bool ShowFriendCount { get; set; }

        /// <summary>
        /// 取得/設定 是否顯示最後一筆的對話
        /// </summary>
        public bool ShowLastChat { get; set; }
    }
}